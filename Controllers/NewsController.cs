using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;

namespace PindahWebsite3.Controllers;

public class NewsController : Controller
{
    private readonly PindahWebsite3Context _context;
    private readonly ILogger<NewsController> _logger;

    public NewsController(PindahWebsite3Context context, ILogger<NewsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var articles = await _context.News
            .OrderByDescending(n => n.DateCreated)
            .ToListAsync();
        
        ViewData["Title"] = "Enterprise Software News & Insights | Pindah Blog";
        ViewData["Description"] = "Latest insights on ERP, CRM, Manufacturing, Insurance, and digital transformation. Real-world case studies, ROI metrics, and implementation best practices for Zimbabwean businesses.";
        ViewData["Keywords"] = "enterprise software, ERP, CRM, digital transformation, Zimbabwe business, software implementation, case studies, ROI, business automation";
        
        var structuredData = new Dictionary<string, object>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "Blog",
            ["name"] = "Pindah Enterprise Software Blog",
            ["description"] = "Latest insights on enterprise software implementation, digital transformation, and business automation for Zimbabwean companies.",
            ["url"] = Url.Action("Index", "News", null, Request.Scheme)!,
            ["blogPost"] = articles.Select(a => new Dictionary<string, object>
            {
                ["@type"] = "BlogPosting",
                ["headline"] = a.Heading,
                ["url"] = Url.Action("Details", "News", new { slug = a.Slug }, Request.Scheme)!,
                ["datePublished"] = a.DateCreated.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                ["author"] = new Dictionary<string, object>
                {
                    ["@type"] = "Organization",
                    ["name"] = "Pindah Private Limited"
                },
                ["publisher"] = new Dictionary<string, object>
                {
                    ["@type"] = "Organization",
                    ["name"] = "Pindah Private Limited",
                    ["logo"] = new Dictionary<string, object>
                    {
                        ["@type"] = "ImageObject",
                        ["url"] = "https://storage.pindah.org/IMAGES/pindah_logo_webp.webp"
                    }
                },
                ["image"] = a.CoverImageUrl ?? string.Empty
            }).ToArray()
        };
        
        ViewData["StructuredData"] = System.Text.Json.JsonSerializer.Serialize(structuredData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        
        return View(articles);
    }

    public async Task<IActionResult> Details(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var article = await _context.News
            .FirstOrDefaultAsync(n => n.Slug == slug);

        if (article == null)
        {
            return NotFound();
        }
        
        ViewData["Title"] = $"{article.Heading} | Pindah Blog";
        ViewData["Description"] = article.Content.Length > 160 ? article.Content.Substring(0, 157) + "..." : article.Content;
        ViewData["Keywords"] = "enterprise software, ERP, CRM, digital transformation, Zimbabwe, business automation, case study";
        ViewData["CanonicalUrl"] = Url.Action("Details", "News", new { slug = article.Slug }, Request.Scheme);
        
        var structuredData = new Dictionary<string, object>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "Article",
            ["headline"] = article.Heading,
            ["description"] = article.Content.Length > 160 ? article.Content.Substring(0, 157) + "..." : article.Content,
            ["url"] = Url.Action("Details", "News", new { slug = article.Slug }, Request.Scheme)!,
            ["datePublished"] = article.DateCreated.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            ["dateModified"] = article.DateCreated.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            ["author"] = new Dictionary<string, object>
            {
                ["@type"] = "Organization",
                ["name"] = "Pindah Private Limited",
                ["url"] = "https://pindah.org"
            },
            ["publisher"] = new Dictionary<string, object>
            {
                ["@type"] = "Organization",
                ["name"] = "Pindah Private Limited",
                ["logo"] = new Dictionary<string, object>
                {
                    ["@type"] = "ImageObject",
                    ["url"] = "https://storage.pindah.org/IMAGES/pindah_logo_webp.webp",
                    ["width"] = 200,
                    ["height"] = 60
                }
            },
            ["image"] = article.CoverImageUrl ?? string.Empty,
            ["mainEntityOfPage"] = new Dictionary<string, object>
            {
                ["@type"] = "WebPage",
                ["@id"] = Url.Action("Details", "News", new { slug = article.Slug }, Request.Scheme)!
            }
        };
        
        ViewData["StructuredData"] = System.Text.Json.JsonSerializer.Serialize(structuredData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

        return View(article);
    }
}
