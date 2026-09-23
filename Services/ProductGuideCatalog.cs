using System.Text.RegularExpressions;
using Markdig;
using PindahWebsite3.ViewModels;

namespace PindahWebsite3.Services;

public sealed class ProductGuideCatalog
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UseAutoIdentifiers()
        .Build();

    private static readonly Dictionary<string, string> TitleOverrides = new(StringComparer.OrdinalIgnoreCase)
    {
        ["assets"] = "Asset Management",
        ["authentication"] = "Authentication & Security",
        ["clinic-healthcare"] = "Healthcare (Clinic)",
        ["forms"] = "Forms & Surveys",
        ["guides-help"] = "Guides & Help",
        ["hms"] = "Hospital Management System (HMS)",
        ["hr-payroll"] = "HR & Payroll",
        ["insurance"] = "Insurance Management Software",
        ["inventory-stock"] = "Inventory & Stock Management",
        ["mobile-scanner"] = "Mobile Scanner",
        ["pharmacy-delivery"] = "Pharmacy Home Delivery",
        ["all-modules"] = "Pindah Basa — All Modules",
        ["full-tour"] = "Pindah Basa — Full Tour (Buyer's Guide)",
        ["school"] = "School Management (Frame)",
        ["agriculture"] = "Agriculture & Feed",
        ["accounting"] = "Accounting & Financial Management",
        ["core-platform"] = "Core Platform",
        ["account-settings"] = "Account Settings",
    };

    private readonly string _docsRoot;

    public ProductGuideCatalog(IWebHostEnvironment env)
    {
        _docsRoot = Path.Combine(env.WebRootPath, "product-guides");
    }

    public IReadOnlyList<ProductGuideListItem> ListGuides()
    {
        if (!Directory.Exists(_docsRoot))
        {
            return Array.Empty<ProductGuideListItem>();
        }

        return Directory.EnumerateFiles(_docsRoot, "*.md", SearchOption.TopDirectoryOnly)
            .Select(path =>
            {
                var slug = Path.GetFileNameWithoutExtension(path);
                var markdown = File.ReadAllText(path);
                return new ProductGuideListItem
                {
                    Slug = slug,
                    Title = ResolveTitle(slug, markdown),
                    Summary = ResolveSummary(markdown),
                    UpdatedUtc = File.GetLastWriteTimeUtc(path),
                    HasPdf = File.Exists(Path.Combine(_docsRoot, $"{slug}.pdf"))
                };
            })
            .OrderBy(g => g.Title, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public ProductGuideDetailViewModel? GetGuide(string slug)
    {
        if (!IsSafeSlug(slug))
        {
            return null;
        }

        var path = Path.Combine(_docsRoot, $"{slug}.md");
        if (!File.Exists(path))
        {
            return null;
        }

        var markdown = File.ReadAllText(path);
        markdown = RewriteLocalImagePaths(markdown);
        var html = Markdown.ToHtml(StripYamlFrontMatter(markdown), Pipeline);

        return new ProductGuideDetailViewModel
        {
            Slug = slug,
            Title = ResolveTitle(slug, markdown),
            Html = html,
            UpdatedUtc = File.GetLastWriteTimeUtc(path),
            HasPdf = File.Exists(Path.Combine(_docsRoot, $"{slug}.pdf"))
        };
    }

    public string? GetPdfPath(string slug)
    {
        if (!IsSafeSlug(slug))
        {
            return null;
        }

        var path = Path.Combine(_docsRoot, $"{slug}.pdf");
        return File.Exists(path) ? path : null;
    }

    private static bool IsSafeSlug(string slug) =>
        !string.IsNullOrWhiteSpace(slug)
        && slug.IndexOfAny(['/', '\\', '.', ':']) < 0
        && Regex.IsMatch(slug, @"^[a-z0-9\-]+$", RegexOptions.IgnoreCase);

    private static string ResolveTitle(string slug, string markdown)
    {
        if (TitleOverrides.TryGetValue(slug, out var overrideTitle))
        {
            return overrideTitle;
        }

        var match = Regex.Match(StripYamlFrontMatter(markdown), @"^#\s+(.+)$", RegexOptions.Multiline);
        if (match.Success)
        {
            return match.Groups[1].Value.Trim();
        }

        return slug.Replace('-', ' ');
    }

    private static string ResolveSummary(string markdown)
    {
        var body = StripYamlFrontMatter(markdown);

        var exec = Regex.Match(
            body,
            @"##\s+Executive Summary\s*\r?\n+(.+?)(?=\r?\n##|\z)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
        if (exec.Success)
        {
            return TruncatePlain(exec.Groups[1].Value, 180);
        }

        // Prefer the first substantive paragraph after the H1.
        var paragraphs = Regex.Matches(body, @"^(?!#)(?!\s*\|)(?!\s*!\[)(.+)$", RegexOptions.Multiline)
            .Select(m => m.Groups[1].Value.Trim())
            .Where(p => p.Length > 40 && !p.StartsWith("---", StringComparison.Ordinal))
            .Take(2)
            .ToList();

        if (paragraphs.Count > 0)
        {
            return TruncatePlain(string.Join(' ', paragraphs), 180);
        }

        return "Pindah Basa module user guide.";
    }

    private static string TruncatePlain(string text, int max)
    {
        var plain = Regex.Replace(text, @"[*_`\[\]#>]", string.Empty);
        plain = Regex.Replace(plain, @"\s+", " ").Trim();
        if (plain.Length <= max)
        {
            return plain;
        }

        return plain[..(max - 1)].TrimEnd() + "…";
    }

    private static string StripYamlFrontMatter(string markdown)
    {
        if (!markdown.StartsWith("---", StringComparison.Ordinal))
        {
            return markdown;
        }

        var end = markdown.IndexOf("---", 3, StringComparison.Ordinal);
        return end < 0 ? markdown : markdown[(end + 3)..].TrimStart();
    }

    private static string RewriteLocalImagePaths(string markdown) =>
        Regex.Replace(
            markdown,
            @"\!\[([^\]]*)\]\(\./screenshots/([^)]+)\)",
            "![$1](/product-guides/screenshots/$2)");
}
