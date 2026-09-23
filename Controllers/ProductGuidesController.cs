using Microsoft.AspNetCore.Mvc;
using PindahWebsite3.Services;

namespace PindahWebsite3.Controllers;

public class ProductGuidesController : Controller
{
<<<<<<< Updated upstream
    private readonly ProductGuideService _guides;

    public ProductGuidesController(ProductGuideService guides)
    {
        _guides = guides;
=======
    private readonly ProductGuideCatalog _catalog;

    public ProductGuidesController(ProductGuideCatalog catalog)
    {
        _catalog = catalog;
>>>>>>> Stashed changes
    }

    [HttpGet("/product-guides")]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
    public IActionResult Index()
    {
<<<<<<< Updated upstream
        var guides = _guides.ListGuides();

        ViewData["Title"] = "Product Guides | Pindah Basa User Manuals";
        ViewData["Description"] = "Read Pindah Basa product guides for accounting, inventory, pharmacy, school, HR, and other modules. Step-by-step markdown manuals from Pindah Private Limited.";
        ViewData["Keywords"] = "Pindah product guides, Basa user manual, accounting guide Zimbabwe, inventory stock guide, ERP documentation Pindah";
=======
        var guides = _catalog.ListGuides();

        ViewData["Title"] = "Product Guides | Pindah Basa User Manuals";
        ViewData["Description"] = "Read-only Pindah Basa module manuals — accounting, inventory, pharmacy, school, and more. Hosted as markdown; displayed as formatted guides.";
        ViewData["Keywords"] = "Pindah Basa user guide, product documentation, ERP manual Zimbabwe, pharmacy guide, school management guide, accounting software guide";
>>>>>>> Stashed changes
        ViewData["CanonicalUrl"] = "https://pindah.org/product-guides";

        return View(guides);
    }

<<<<<<< Updated upstream
=======
    [HttpGet("/product-guides/{slug}")]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
    public IActionResult Guide(string slug)
    {
        var guide = _catalog.GetGuide(slug);
        if (guide is null)
        {
            return NotFound();
        }

        ViewData["Title"] = $"{guide.Title} | Pindah Basa Product Guide";
        ViewData["Description"] = $"{guide.Title} — read-only Pindah Basa user documentation.";
        ViewData["Keywords"] = $"{guide.Title}, Pindah Basa guide, product documentation";
        ViewData["CanonicalUrl"] = $"https://pindah.org/product-guides/{guide.Slug}";
        ViewData["OgType"] = "article";

        return View(guide);
    }

>>>>>>> Stashed changes
    [HttpGet("/product-guides/{slug}.pdf")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public IActionResult Pdf(string slug)
    {
<<<<<<< Updated upstream
        var path = _guides.GetPdfPath(slug);
        if (path == null)
=======
        var path = _catalog.GetPdfPath(slug);
        if (path is null)
>>>>>>> Stashed changes
        {
            return NotFound();
        }

<<<<<<< Updated upstream
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
=======
        return PhysicalFile(path, "application/pdf", enableRangeProcessing: true);
>>>>>>> Stashed changes
    }
}
