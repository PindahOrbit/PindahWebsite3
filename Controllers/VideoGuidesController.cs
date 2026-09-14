using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;

namespace PindahWebsite3.Controllers;

public class VideoGuidesController : Controller
{
    private readonly PindahWebsite3Context _context;

    public VideoGuidesController(PindahWebsite3Context context)
    {
        _context = context;
    }

    [HttpGet("/video-guides")]
    public async Task<IActionResult> Index()
    {
        var guides = await _context.VideoGuides
            .AsNoTracking()
            .Where(v => v.IsPublished)
            .OrderBy(v => v.SortOrder)
            .ThenByDescending(v => v.DateAdded)
            .ToListAsync();

        ViewData["Title"] = "Video Guides | Pindah Product Tutorials";
        ViewData["Description"] = "Watch Pindah video guides for ERP, BasaRx, Frame, accounting, and other enterprise software products. Step-by-step YouTube tutorials from Pindah Private Limited.";
        ViewData["Keywords"] = "Pindah video guides, ERP tutorial Zimbabwe, BasaRx training, Frame school software videos, Pindah how-to videos";
        ViewData["CanonicalUrl"] = "https://pindah.org/video-guides";

        return View(guides);
    }
}
