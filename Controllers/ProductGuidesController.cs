using Microsoft.AspNetCore.Mvc;
using PindahWebsite3.Services;

namespace PindahWebsite3.Controllers;

public class ProductGuidesController : Controller
{
    private readonly ProductGuideService _guides;

    public ProductGuidesController(ProductGuideService guides)
    {
        _guides = guides;
    }

    [HttpGet("/product-guides")]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
    public IActionResult Index()
    {
        var guides = _guides.ListGuides();

        ViewData["Title"] = "Product Guides | Pindah Basa User Manuals";
        ViewData["Description"] = "Read Pindah Basa product guides for accounting, inventory, pharmacy, school, HR, and other modules. Step-by-step markdown manuals from Pindah Private Limited.";
        ViewData["Keywords"] = "Pindah product guides, Basa user manual, accounting guide Zimbabwe, inventory stock guide, ERP documentation Pindah";
        ViewData["CanonicalUrl"] = "https://pindah.org/product-guides";

        return View(guides);
    }

    [HttpGet("/product-guides/{slug}.pdf")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public IActionResult Pdf(string slug)
    {
        var path = _guides.GetPdfPath(slug);
        if (path == null)
        {
            return NotFound();
        }

        return PhysicalFile(path, "application/pdf", fileDownloadName: $"{slug}.pdf");
    }

    [HttpGet("/product-guides/{slug}")]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
    public IActionResult Details(string slug)
    {
        if (!string.IsNullOrEmpty(slug) && slug.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return Pdf(slug[..^4]);
        }

        var guide = _guides.GetGuide(slug);
        if (guide == null)
        {
            return NotFound();
        }

        ViewData["Title"] = $"{guide.Title} | Pindah Product Guides";
        ViewData["Description"] = $"Pindah Basa user guide: {guide.Title}. Read-only product documentation from Pindah Private Limited.";
        ViewData["Keywords"] = $"{guide.Title}, Pindah Basa guide, product documentation, {slug}";
        ViewData["CanonicalUrl"] = $"https://pindah.org/product-guides/{slug}";

        return View(guide);
    }
}
