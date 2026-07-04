using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models.ZimsecLibrary;

namespace PindahWebsite3.Services.Zimsec;

public class ZimsecSearchService : IZimsecSearchService
{
    private readonly ZimsecContext _context;
    private readonly IZimsecCatalogService _catalog;
    private readonly IConfiguration _configuration;

    public ZimsecSearchService(ZimsecContext context, IZimsecCatalogService catalog, IConfiguration configuration)
    {
        _context = context;
        _catalog = catalog;
        _configuration = configuration;
    }

    public async Task<ZimsecSearchResult> SearchAsync(
        string? query,
        string? level = null,
        string? subject = null,
        int limit = 40,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Clamp(limit, 1, 100);
        var trimmed = (query ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(trimmed))
        {
            return await BrowseFilteredAsync(level, subject, limit, cancellationToken);
        }

        var terms = ExtractSearchTerms(trimmed);
        if (terms.Count == 0)
            return await BrowseFilteredAsync(level, subject, limit, cancellationToken);

        var ftsQuery = BuildFtsQuery(terms);
        var connectionString = _configuration.GetConnectionString("ZimsecContextConnection")
            ?? "Data Source=zimsec.db";

        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        var hits = new List<ZimsecSearchHit>();
        var levelFacets = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var subjectFacets = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        var sql = """
            SELECT d.Id, d.Title, d.FileName, d.Level, d.SubjectSlug, d.SubjectDisplay,
                   bm25(DocumentSearch) AS rank,
                   snippet(DocumentSearch, 5, '<mark>', '</mark>', '…', 32) AS snippet
            FROM DocumentSearch
            INNER JOIN Documents d ON d.Id = DocumentSearch.DocumentId
            WHERE DocumentSearch MATCH @query
            """;

        if (!string.IsNullOrWhiteSpace(level))
            sql += " AND d.Level = @level";
        if (!string.IsNullOrWhiteSpace(subject))
            sql += " AND d.SubjectSlug = @subject";

        sql += " ORDER BY rank LIMIT @limit";

        await using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@query", ftsQuery);
            if (!string.IsNullOrWhiteSpace(level))
                cmd.Parameters.AddWithValue("@level", level.ToLowerInvariant());
            if (!string.IsNullOrWhiteSpace(subject))
                cmd.Parameters.AddWithValue("@subject", subject.ToLowerInvariant());
            cmd.Parameters.AddWithValue("@limit", limit);

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                var lvl = reader.GetString(3);
                var subj = reader.GetString(4);
                levelFacets[lvl] = levelFacets.GetValueOrDefault(lvl) + 1;
                subjectFacets[subj] = subjectFacets.GetValueOrDefault(subj) + 1;

                hits.Add(new ZimsecSearchHit(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    lvl,
                    _catalog.FormatLevelDisplay(lvl),
                    subj,
                    reader.GetString(5),
                    reader.GetDouble(6),
                    reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                    terms));
            }
        }

        if (hits.Count == 0)
        {
            var fallback = await FallbackContainsSearchAsync(trimmed, level, subject, limit, cancellationToken);
            return fallback;
        }

        return new ZimsecSearchResult(trimmed, hits.Count, hits, levelFacets, subjectFacets);
    }

    private async Task<ZimsecSearchResult> BrowseFilteredAsync(
        string? level, string? subject, int limit, CancellationToken cancellationToken)
    {
        var q = _context.Documents.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(level))
            q = q.Where(d => d.Level == level.ToLowerInvariant());
        if (!string.IsNullOrWhiteSpace(subject))
            q = q.Where(d => d.SubjectSlug == subject.ToLowerInvariant());

        var docs = await q.OrderBy(d => d.SubjectDisplay).ThenBy(d => d.Title).Take(limit).ToListAsync(cancellationToken);
        var hits = docs.Select(d => new ZimsecSearchHit(
            d.Id, d.Title, d.FileName, d.Level, _catalog.FormatLevelDisplay(d.Level),
            d.SubjectSlug, d.SubjectDisplay, 0,
            Truncate(d.ExtractedText, 160), Array.Empty<string>())).ToList();

        return new ZimsecSearchResult(string.Empty, hits.Count, hits,
            hits.GroupBy(h => h.Level).ToDictionary(g => g.Key, g => g.Count()),
            hits.GroupBy(h => h.SubjectSlug).ToDictionary(g => g.Key, g => g.Count()));
    }

    private async Task<ZimsecSearchResult> FallbackContainsSearchAsync(
        string query, string? level, string? subject, int limit, CancellationToken cancellationToken)
    {
        var lower = query.ToLowerInvariant();
        var q = _context.Documents.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(level))
            q = q.Where(d => d.Level == level.ToLowerInvariant());
        if (!string.IsNullOrWhiteSpace(subject))
            q = q.Where(d => d.SubjectSlug == subject.ToLowerInvariant());

        q = q.Where(d =>
            d.Title.ToLower().Contains(lower) ||
            d.FileName.ToLower().Contains(lower) ||
            d.SubjectDisplay.ToLower().Contains(lower) ||
            d.ExtractedText.ToLower().Contains(lower));

        var docs = await q.OrderBy(d => d.Title).Take(limit).ToListAsync(cancellationToken);
        var terms = ExtractSearchTerms(query);
        var hits = docs.Select(d =>
        {
            var snippet = BuildSnippetFromText(d.ExtractedText, lower, terms) ?? Truncate(d.Title, 160);
            return new ZimsecSearchHit(
                d.Id, d.Title, d.FileName, d.Level, _catalog.FormatLevelDisplay(d.Level),
                d.SubjectSlug, d.SubjectDisplay, 0.5, snippet, terms);
        }).ToList();

        return new ZimsecSearchResult(query, hits.Count, hits,
            hits.GroupBy(h => h.Level).ToDictionary(g => g.Key, g => g.Count()),
            hits.GroupBy(h => h.SubjectSlug).ToDictionary(g => g.Key, g => g.Count()));
    }

    internal static List<string> ExtractSearchTerms(string query)
    {
        var terms = new List<string>();
        foreach (Match m in Regex.Matches(query, @"""([^""]+)"""))
            terms.Add(m.Groups[1].Value.Trim());
        var remainder = Regex.Replace(query, @"""[^""]+""", " ");
        foreach (var part in remainder.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var t = part.Trim().Trim('*');
            if (t.Length >= 2) terms.Add(t);
        }
        return terms.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    internal static string BuildFtsQuery(IReadOnlyList<string> terms)
    {
        if (terms.Count == 0) return string.Empty;

        var parts = new List<string>();
        foreach (var term in terms)
        {
            var escaped = term.Replace("\"", "\"\"");
            if (term.Contains(' '))
                parts.Add($"\"{escaped}\"");
            else
                parts.Add($"{escaped}*");
        }

        return string.Join(" AND ", parts);
    }

    private static string? BuildSnippetFromText(string text, string lowerQuery, IReadOnlyList<string> terms)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        var idx = text.IndexOf(lowerQuery, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
        {
            foreach (var t in terms)
            {
                idx = text.IndexOf(t, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0) break;
            }
        }
        if (idx < 0) return Truncate(text, 160);

        var start = Math.Max(0, idx - 60);
        var len = Math.Min(text.Length - start, 180);
        var slice = text.Substring(start, len).Replace('\n', ' ').Replace('\r', ' ');
        if (start > 0) slice = "…" + slice;
        if (start + len < text.Length) slice += "…";
        return slice;
    }

    private static string Truncate(string value, int max)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        value = value.Replace('\n', ' ').Replace('\r', ' ');
        return value.Length <= max ? value : value[..max] + "…";
    }
}
