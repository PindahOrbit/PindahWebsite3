using System.Globalization;
using System.Text;

namespace PindahWebsite3.Services.Zimsec;

public class ZimsecCatalogService : IZimsecCatalogService
{
    private readonly IWebHostEnvironment _env;
    private static readonly HashSet<string> ValidLevels = new(StringComparer.OrdinalIgnoreCase) { "o-level", "a-level" };

    public ZimsecCatalogService(IWebHostEnvironment env) => _env = env;

    public string LibraryRoot => Path.Combine(_env.WebRootPath, "zimsec");

    public IReadOnlyList<ZimsecLevelNode> GetTreeFromDisk()
    {
        var root = LibraryRoot;
        if (!Directory.Exists(root)) return Array.Empty<ZimsecLevelNode>();

        var levels = new List<ZimsecLevelNode>();
        foreach (var levelDir in Directory.EnumerateDirectories(root).OrderBy(d => d))
        {
            var levelSlug = Path.GetFileName(levelDir);
            if (!ValidLevels.Contains(levelSlug)) continue;

            var subjects = new List<ZimsecSubjectNode>();
            foreach (var subjectDir in Directory.EnumerateDirectories(levelDir).OrderBy(d => d))
            {
                var subjectSlug = Path.GetFileName(subjectDir);
                var count = Directory.EnumerateFiles(subjectDir, "*.pdf", SearchOption.TopDirectoryOnly).Count();
                if (count == 0) continue;
                subjects.Add(new ZimsecSubjectNode(subjectSlug, FormatSubjectDisplay(subjectSlug), count));
            }

            if (subjects.Count > 0)
                levels.Add(new ZimsecLevelNode(levelSlug, FormatLevelDisplay(levelSlug), subjects));
        }

        return levels;
    }

    public string FormatLevelDisplay(string slug) => slug.ToLowerInvariant() switch
    {
        "o-level" => "O Level",
        "a-level" => "A Level",
        _ => ToTitle(slug)
    };

    public string FormatSubjectDisplay(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return slug;
        return ToTitle(slug.Replace('-', ' '));
    }

    public string? ResolvePhysicalPath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return null;
        var normalized = relativePath.Replace('\\', '/').TrimStart('/');
        var full = Path.GetFullPath(Path.Combine(LibraryRoot, normalized.Replace('/', Path.DirectorySeparatorChar)));
        var rootFull = Path.GetFullPath(LibraryRoot);
        if (!full.StartsWith(rootFull, StringComparison.OrdinalIgnoreCase)) return null;
        return File.Exists(full) ? full : null;
    }

    private static string ToTitle(string value)
    {
        var ti = CultureInfo.InvariantCulture.TextInfo;
        return ti.ToTitleCase(value.ToLowerInvariant());
    }
}
