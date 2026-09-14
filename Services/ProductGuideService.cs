using System.Text.RegularExpressions;
using Markdig;
using PindahWebsite3.Models;

namespace PindahWebsite3.Services;

/// <summary>
/// Read-only host for product documentation markdown files under wwwroot/product-guides.
/// Does not modify source .md files.
/// </summary>
public class ProductGuideService
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UseAutoIdentifiers()
        .Build();

    private static readonly Regex FrontMatter = new(
        @"^---\s*\r?\n.*?\r?\n---\s*\r?\n?",
        RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex FirstHeading = new(
        @"^#\s+(.+)$",
        RegexOptions.Multiline | RegexOptions.Compiled);

    private static readonly Regex RelativeScreenshot = new(
        @"\!\[([^\]]*)\]\(\./screenshots/([^)\s]+)\)",
        RegexOptions.Compiled);

    private static readonly Regex RelativeLocalImage = new(
        @"\!\[([^\]]*)\]\(\./([^)\s]+\.(?:png|jpg|jpeg|webp|gif))\)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private readonly IWebHostEnvironment _env;
    private readonly string _guidesRoot;

    public ProductGuideService(IWebHostEnvironment env)
    {
        _env = env;
        _guidesRoot = Path.Combine(_env.WebRootPath, "product-guides");
    }

    public IReadOnlyList<ProductGuideInfo> ListGuides()
    {
        if (!Directory.Exists(_guidesRoot))
        {
            return Array.Empty<ProductGuideInfo>();
        }

        return Directory.EnumerateFiles(_guidesRoot, "*.md", SearchOption.TopDirectoryOnly)
            .Select(path =>
            {
                var slug = Path.GetFileNameWithoutExtension(path);
                var text = File.ReadAllText(path);
                var body = StripFrontMatter(text);
                var hasPdf = File.Exists(Path.Combine(_guidesRoot, $"{slug}.pdf"));
                return new ProductGuideInfo
                {
                    Slug = slug,
                    Title = ExtractTitle(body, slug),
                    Summary = ExtractSummary(body),
                    LastModifiedUtc = File.GetLastWriteTimeUtc(path),
                    HasPdf = hasPdf,
                    PdfUrl = hasPdf ? $"/product-guides/{slug}.pdf" : null
                };
            })
            .OrderBy(g => g.Title, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public ProductGuideViewModel? GetGuide(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug) || slug.Contains("..") || slug.Contains('/') || slug.Contains('\\'))
        {
            return null;
        }

        var path = Path.Combine(_guidesRoot, $"{slug}.md");
        if (!File.Exists(path))
        {
            return null;
        }

        var text = File.ReadAllText(path);
        var body = StripFrontMatter(text);
        body = RewriteImagePaths(body);
        var title = ExtractTitle(body, slug);
        var html = Markdown.ToHtml(body, Pipeline);
        var hasPdf = File.Exists(Path.Combine(_guidesRoot, $"{slug}.pdf"));

        return new ProductGuideViewModel
        {
            Slug = slug,
            Title = title,
            Html = html,
            LastModifiedUtc = File.GetLastWriteTimeUtc(path),
            HasPdf = hasPdf,
            PdfUrl = hasPdf ? $"/product-guides/{slug}.pdf" : null
        };
    }

    public string? GetPdfPath(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug) || slug.Contains("..") || slug.Contains('/') || slug.Contains('\\'))
        {
            return null;
        }

        var path = Path.Combine(_guidesRoot, $"{slug}.pdf");
        return File.Exists(path) ? path : null;
    }

    private static string StripFrontMatter(string markdown)
    {
        return FrontMatter.Replace(markdown, string.Empty);
    }

    private static string ExtractTitle(string markdown, string fallbackSlug)
    {
        var match = FirstHeading.Match(markdown);
        if (match.Success)
        {
            return match.Groups[1].Value.Trim();
        }

        return fallbackSlug
            .Replace('-', ' ')
            .Replace('_', ' ');
    }

    private static string? ExtractSummary(string markdown)
    {
        var withoutHeading = FirstHeading.Replace(markdown, string.Empty, 1);
        foreach (var block in withoutHeading.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries))
        {
            var line = block.Trim();
            if (line.Length == 0 || line.StartsWith('#') || line.StartsWith('|') || line.StartsWith('!') || line.StartsWith("---"))
            {
                continue;
            }

            if (line.StartsWith("**") && line.Contains("Publisher", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var plain = Regex.Replace(line, @"[*_`#>\[\]]+", string.Empty).Trim();
            if (plain.Length < 40)
            {
                continue;
            }

            return plain.Length > 180 ? plain[..177] + "…" : plain;
        }

        return null;
    }

    private static string RewriteImagePaths(string markdown)
    {
        var rewritten = RelativeScreenshot.Replace(
            markdown,
            "![$1](/product-guides/screenshots/$2)");

        rewritten = RelativeLocalImage.Replace(
            rewritten,
            "![$1](/product-guides/$2)");

        return rewritten;
    }
}
