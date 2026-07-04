using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models.ZimsecLibrary;
using UglyToad.PdfPig;

namespace PindahWebsite3.Services.Zimsec;

public interface IZimsecLibraryIndexer
{
    Task<ZimsecIndexReport> SyncAsync(CancellationToken cancellationToken = default);
}

public record ZimsecIndexReport(int Added, int Updated, int Removed, int Total, int Failed);

public class ZimsecLibraryIndexer : IZimsecLibraryIndexer
{
    private readonly ZimsecContext _context;
    private readonly IZimsecCatalogService _catalog;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ZimsecLibraryIndexer> _logger;

    public ZimsecLibraryIndexer(
        ZimsecContext context,
        IZimsecCatalogService catalog,
        IConfiguration configuration,
        ILogger<ZimsecLibraryIndexer> logger)
    {
        _context = context;
        _catalog = catalog;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ZimsecIndexReport> SyncAsync(CancellationToken cancellationToken = default)
    {
        var root = _catalog.LibraryRoot;
        if (!Directory.Exists(root))
            return new ZimsecIndexReport(0, 0, 0, 0, 0);

        var diskFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var level in new[] { "o-level", "a-level" })
        {
            var levelPath = Path.Combine(root, level);
            if (!Directory.Exists(levelPath)) continue;
            foreach (var subjectDir in Directory.EnumerateDirectories(levelPath))
            {
                var subjectSlug = Path.GetFileName(subjectDir).ToLowerInvariant();
                foreach (var pdf in Directory.EnumerateFiles(subjectDir, "*.pdf"))
                {
                    var rel = $"{level}/{subjectSlug}/{Path.GetFileName(pdf)}".Replace('\\', '/');
                    diskFiles[rel] = pdf;
                }
            }
        }

        var existing = await _context.Documents.ToDictionaryAsync(d => d.RelativePath, StringComparer.OrdinalIgnoreCase, cancellationToken);
        var added = 0;
        var updated = 0;
        var failed = 0;

        foreach (var (relativePath, fullPath) in diskFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var parts = relativePath.Split('/');
                if (parts.Length < 3) continue;
                var level = parts[0].ToLowerInvariant();
                var subjectSlug = parts[1].ToLowerInvariant();
                var fileName = parts[2];
                var hash = await ComputeFileHashAsync(fullPath, cancellationToken);
                var fileInfo = new FileInfo(fullPath);

                if (existing.TryGetValue(relativePath, out var doc))
                {
                    if (doc.ContentHash == hash) continue;
                    doc.FileSizeBytes = fileInfo.Length;
                    doc.ContentHash = hash;
                    doc.IndexedAtUtc = DateTime.UtcNow;
                    await ExtractIntoDocumentAsync(doc, fullPath, cancellationToken);
                    await UpsertFtsAsync(doc, cancellationToken);
                    updated++;
                }
                else
                {
                    doc = new ZimsecLibraryDocument
                    {
                        RelativePath = relativePath,
                        FileName = fileName,
                        Title = Path.GetFileNameWithoutExtension(fileName),
                        Level = level,
                        SubjectSlug = subjectSlug,
                        SubjectDisplay = _catalog.FormatSubjectDisplay(subjectSlug),
                        FileSizeBytes = fileInfo.Length,
                        ContentHash = hash,
                        IndexedAtUtc = DateTime.UtcNow
                    };
                    await ExtractIntoDocumentAsync(doc, fullPath, cancellationToken);
                    _context.Documents.Add(doc);
                    await _context.SaveChangesAsync(cancellationToken);
                    await UpsertFtsAsync(doc, cancellationToken);
                    added++;
                }
            }
            catch (Exception ex)
            {
                failed++;
                _logger.LogWarning(ex, "Failed to index {Path}", relativePath);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        var removed = 0;
        foreach (var stale in existing.Values.Where(d => !diskFiles.ContainsKey(d.RelativePath)).ToList())
        {
            await DeleteFtsAsync(stale.Id, cancellationToken);
            _context.Documents.Remove(stale);
            removed++;
        }

        if (removed > 0)
            await _context.SaveChangesAsync(cancellationToken);

        var total = await _context.Documents.CountAsync(cancellationToken);
        return new ZimsecIndexReport(added, updated, removed, total, failed);
    }

    private static async Task<string> ComputeFileHashAsync(string path, CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(path);
        var hash = await SHA256.HashDataAsync(stream, cancellationToken);
        return Convert.ToHexString(hash);
    }

    private static async Task ExtractIntoDocumentAsync(ZimsecLibraryDocument doc, string fullPath, CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        var pages = 0;
        try
        {
            await Task.Run(() =>
            {
                using var pdf = PdfDocument.Open(fullPath);
                pages = pdf.NumberOfPages;
                foreach (var page in pdf.GetPages())
                    sb.AppendLine(page.Text);
            }, cancellationToken);
        }
        catch
        {
            sb.Clear();
        }

        doc.PageCount = pages;
        doc.ExtractedText = sb.ToString();
        if (doc.ExtractedText.Length > 500_000)
            doc.ExtractedText = doc.ExtractedText[..500_000];
    }

    private async Task UpsertFtsAsync(ZimsecLibraryDocument doc, CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("ZimsecContextConnection")
            ?? "Data Source=zimsec.db";
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using (var del = connection.CreateCommand())
        {
            del.CommandText = "DELETE FROM DocumentSearch WHERE DocumentId = @id";
            del.Parameters.AddWithValue("@id", doc.Id);
            await del.ExecuteNonQueryAsync(cancellationToken);
        }

        await using var insert = connection.CreateCommand();
        insert.CommandText = """
            INSERT INTO DocumentSearch (DocumentId, Title, Subject, Level, FileName, Body)
            VALUES (@id, @title, @subject, @level, @file, @body)
            """;
        insert.Parameters.AddWithValue("@id", doc.Id);
        insert.Parameters.AddWithValue("@title", doc.Title);
        insert.Parameters.AddWithValue("@subject", doc.SubjectDisplay);
        insert.Parameters.AddWithValue("@level", doc.Level);
        insert.Parameters.AddWithValue("@file", doc.FileName);
        insert.Parameters.AddWithValue("@body", doc.ExtractedText);
        await insert.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task DeleteFtsAsync(int documentId, CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("ZimsecContextConnection")
            ?? "Data Source=zimsec.db";
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM DocumentSearch WHERE DocumentId = @id";
        cmd.Parameters.AddWithValue("@id", documentId);
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }
}
