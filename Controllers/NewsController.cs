using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;
using PindahWebsite3.Services;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PindahWebsite3.Controllers;

public class NewsController : Controller
{
    private readonly PindahWebsite3Context _context;
    private readonly OllamaChatService _ollamaChatService;
    private readonly ILogger<NewsController> _logger;

    public NewsController(
        PindahWebsite3Context context,
        OllamaChatService ollamaChatService,
        ILogger<NewsController> logger)
    {
        _context = context;
        _ollamaChatService = ollamaChatService;
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

    [Authorize]
    public IActionResult Generate()
    {
        ViewData["Title"] = "Generate News Article | Pindah Blog";
        ViewData["Description"] = "Create and publish enterprise software news articles with AI-assisted drafting.";
        ViewData["Robots"] = "noindex, nofollow";
        return View(new NewsSaveModel());
    }

    [Authorize]
    [HttpGet]
    public async Task Stream([FromQuery] string field, [FromQuery] string? heading, CancellationToken cancellationToken)
    {
        var prompt = field?.ToLowerInvariant() switch
        {
            "heading" => NewsPrompts.Heading,
            "content" when !string.IsNullOrWhiteSpace(heading) => NewsPrompts.Content(heading),
            "keyword" when !string.IsNullOrWhiteSpace(heading) => NewsPrompts.ImageKeyword(heading),
            _ => null
        };

        if (prompt == null)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync("Invalid stream field or missing heading.", cancellationToken);
            return;
        }

        await StreamPromptAsync(prompt, field, cancellationToken);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task StreamConversation([FromBody] NewsConversationRequest request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync("Missing conversation request.", cancellationToken);
            return;
        }

        var target = request.Target?.ToLowerInvariant();
        var instruction = request.Instruction?.Trim();

        if (string.IsNullOrWhiteSpace(instruction))
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync("Missing edit instruction.", cancellationToken);
            return;
        }

        var prompt = target switch
        {
            "heading" when !string.IsNullOrWhiteSpace(request.Heading) =>
                NewsPrompts.ReviseHeading(request.Heading, instruction),
            "content" when !string.IsNullOrWhiteSpace(request.Heading) && !string.IsNullOrWhiteSpace(request.Content) =>
                NewsPrompts.ReviseContent(request.Heading, request.Content, instruction),
            _ => null
        };

        if (prompt == null)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync("Invalid conversation target or missing draft content.", cancellationToken);
            return;
        }

        await StreamPromptAsync(prompt, target ?? "conversation", cancellationToken);
    }

    private async Task StreamPromptAsync(string prompt, string field, CancellationToken cancellationToken)
    {
        Response.StatusCode = StatusCodes.Status200OK;
        Response.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";
        Response.Headers.Append("X-Accel-Buffering", "no");

        try
        {
            await foreach (var chunk in _ollamaChatService.StreamGenerateAsync(prompt, cancellationToken))
            {
                var payload = JsonSerializer.Serialize(new
                {
                    content = chunk.Content,
                    thinking = chunk.Thinking,
                    done = chunk.Done
                });

                await Response.WriteAsync($"data: {payload}\n\n", cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }

            await Response.WriteAsync("data: {\"done\":true}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("News stream cancelled for field {Field}", field);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "News stream failed for field {Field}", field);
            var errorPayload = JsonSerializer.Serialize(new { error = "Generation failed. Please try again." });
            await Response.WriteAsync($"data: {errorPayload}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save([FromForm] NewsSaveModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please complete all required fields before publishing.";
            return View("Generate", model);
        }

        var slug = string.IsNullOrWhiteSpace(model.Slug)
            ? GenerateSlug(model.Heading)
            : GenerateSlug(model.Slug);

        var originalSlug = slug;
        var counter = 1;
        while (await _context.News.AnyAsync(n => n.Slug == slug, cancellationToken))
        {
            slug = $"{originalSlug}-{counter}";
            counter++;
        }

        var news = new News
        {
            Heading = model.Heading.Trim(),
            Content = model.Content.Trim(),
            CoverImageUrl = model.CoverImageUrl.Trim(),
            Slug = slug,
            DateCreated = DateTime.UtcNow
        };

        _context.News.Add(news);
        await _context.SaveChangesAsync(cancellationToken);

        return RedirectToAction(nameof(Details), new { slug });
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
        var plainDescription = StripHtml(article.Content);
        ViewData["Description"] = plainDescription.Length > 160 ? plainDescription[..157] + "..." : plainDescription;
        ViewData["Keywords"] = "enterprise software, ERP, CRM, digital transformation, Zimbabwe, business automation, case study";
        ViewData["CanonicalUrl"] = Url.Action("Details", "News", new { slug = article.Slug }, Request.Scheme);
        
        var structuredData = new Dictionary<string, object>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "Article",
            ["headline"] = article.Heading,
            ["description"] = plainDescription.Length > 160 ? plainDescription[..157] + "..." : plainDescription,
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

    private static string StripHtml(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var text = Regex.Replace(input, "<[^>]+>", " ");
        return Regex.Replace(text, @"\s+", " ").Trim();
    }

    private static string GenerateSlug(string input)
    {
        var slug = input.ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"-+", "-");
        slug = slug.Trim('-');

        if (slug.Length > 80)
        {
            slug = slug[..80].TrimEnd('-');
        }

        var hash = Guid.NewGuid().ToString("N")[..6];
        return $"{slug}-{hash}";
    }
}
