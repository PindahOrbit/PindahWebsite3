using Microsoft.Extensions.Options;

namespace PindahWebsite3.Services;

/// <summary>
/// Copies product documentation (markdown, PDFs, screenshots) into wwwroot/product-guides.
/// </summary>
public class ProductGuideSyncService
{
    private readonly IWebHostEnvironment _env;
    private readonly IOptionsMonitor<ProductGuidesOptions> _options;
    private readonly ILogger<ProductGuideSyncService> _logger;
    private readonly object _syncLock = new();

    public ProductGuideSyncService(
        IWebHostEnvironment env,
        IOptionsMonitor<ProductGuidesOptions> options,
        ILogger<ProductGuideSyncService> logger)
    {
        _env = env;
        _options = options;
        _logger = logger;
    }

    public string DestinationRoot => Path.Combine(_env.WebRootPath, "product-guides");

    public string? ResolveSourcePath()
    {
        var configured = _options.CurrentValue.SourcePath?.Trim();
        if (string.IsNullOrWhiteSpace(configured))
        {
            return null;
        }

        var path = Path.IsPathRooted(configured)
            ? configured
            : Path.GetFullPath(Path.Combine(_env.ContentRootPath, configured));

        return path;
    }

    /// <summary>Copy newer/changed files from source → wwwroot/product-guides.</summary>
    public int SyncOnce()
    {
        if (!_options.CurrentValue.Enabled)
        {
            return 0;
        }

        var source = ResolveSourcePath();
        if (string.IsNullOrWhiteSpace(source) || !Directory.Exists(source))
        {
            _logger.LogDebug("Product guide source not found: {Source}", source);
            return 0;
        }

        lock (_syncLock)
        {
            var dest = DestinationRoot;
            Directory.CreateDirectory(dest);

            var copied = 0;
            foreach (var pattern in new[] { "*.md", "*.pdf" })
            {
                foreach (var file in Directory.EnumerateFiles(source, pattern, SearchOption.TopDirectoryOnly))
                {
                    if (CopyIfNewer(file, Path.Combine(dest, Path.GetFileName(file))))
                    {
                        copied++;
                    }
                }
            }

            var sourceShots = Path.Combine(source, "screenshots");
            var destShots = Path.Combine(dest, "screenshots");
            if (Directory.Exists(sourceShots))
            {
                copied += CopyDirectoryNewer(sourceShots, destShots);
            }

            if (copied > 0)
            {
                _logger.LogInformation("Product guides sync copied/updated {Count} file(s) from {Source}", copied, source);
            }

            return copied;
        }
    }

    private static int CopyDirectoryNewer(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);
        var count = 0;

        foreach (var file in Directory.EnumerateFiles(sourceDir, "*", SearchOption.TopDirectoryOnly))
        {
            if (CopyIfNewer(file, Path.Combine(destDir, Path.GetFileName(file))))
            {
                count++;
            }
        }

        foreach (var sub in Directory.EnumerateDirectories(sourceDir, "*", SearchOption.TopDirectoryOnly))
        {
            count += CopyDirectoryNewer(sub, Path.Combine(destDir, Path.GetFileName(sub)));
        }

        return count;
    }

    private static bool CopyIfNewer(string sourceFile, string destFile)
    {
        var srcInfo = new FileInfo(sourceFile);
        if (!srcInfo.Exists)
        {
            return false;
        }

        var destInfo = new FileInfo(destFile);
        if (destInfo.Exists
            && destInfo.Length == srcInfo.Length
            && destInfo.LastWriteTimeUtc >= srcInfo.LastWriteTimeUtc)
        {
            return false;
        }

        var destParent = Path.GetDirectoryName(destFile);
        if (!string.IsNullOrEmpty(destParent))
        {
            Directory.CreateDirectory(destParent);
        }

        File.Copy(sourceFile, destFile, overwrite: true);
        try
        {
            File.SetLastWriteTimeUtc(destFile, srcInfo.LastWriteTimeUtc);
        }
        catch
        {
            // Best-effort timestamp preserve
        }

        return true;
    }
}
