using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PindahWebsite3.Services;

namespace PindahWebsite3.Controllers;

public class SeoController : Controller
{
    [HttpGet]
    public IActionResult Landing(string slug)
    {
        if (!SeoLandingCatalog.TryGet(slug, out var page))
        {
            return NotFound();
        }

        var canonical = $"https://pindah.org/{page.Slug}";
        ViewData["Title"] = page.Title;
        ViewData["Description"] = page.Description;
        ViewData["Keywords"] = page.Keywords;
        ViewData["CanonicalUrl"] = canonical;
        ViewData["OgImage"] = "https://storage.pindah.org/IMAGES/pindah-og-default.jpg";
        ViewData["StructuredData"] = BuildStructuredData(page, canonical);

        return View("Landing", page);
    }

    private static string BuildStructuredData(Models.SeoLandingPage page, string canonical)
    {
        var graph = new List<object>
        {
            new
            {
                @type = "WebPage",
                name = page.H1,
                description = page.Description,
                url = canonical,
                isPartOf = new { @id = "https://pindah.org/#website" },
                about = new { @id = "https://pindah.org/#organization" }
            },
            new
            {
                @type = "BreadcrumbList",
                itemListElement = new object[]
                {
                    new { @type = "ListItem", position = 1, name = "Home", item = "https://pindah.org" },
                    new { @type = "ListItem", position = 2, name = page.H1, item = canonical }
                }
            },
            new
            {
                @type = "SoftwareApplication",
                name = $"Pindah — {page.H1}",
                applicationCategory = "BusinessApplication",
                operatingSystem = "Web",
                description = page.Description,
                offers = new
                {
                    @type = "Offer",
                    price = "0",
                    priceCurrency = "USD",
                    description = "Contact for enterprise pricing"
                },
                publisher = new { @type = "Organization", name = "Pindah Private Limited", url = "https://pindah.org" }
            }
        };

        if (page.Faqs.Count > 0)
        {
            graph.Add(new
            {
                @type = "FAQPage",
                mainEntity = page.Faqs.Select(f => new
                {
                    @type = "Question",
                    name = f.Question,
                    acceptedAnswer = new { @type = "Answer", text = f.Answer }
                }).ToArray()
            });
        }

        return JsonSerializer.Serialize(new { @context = "https://schema.org", @graph = graph },
            new JsonSerializerOptions { WriteIndented = true });
    }
}
