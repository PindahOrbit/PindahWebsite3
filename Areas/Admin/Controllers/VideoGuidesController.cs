using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;
using PindahWebsite3.Services;

namespace PindahWebsite3.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = CmsConstants.RoleAdmin)]
public class VideoGuidesController : Controller
{
    private readonly PindahWebsite3Context _context;

    public VideoGuidesController(PindahWebsite3Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var guides = await _context.VideoGuides
            .OrderBy(v => v.SortOrder)
            .ThenByDescending(v => v.DateAdded)
            .ToListAsync();
        return View(guides);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VideoGuideSaveModel model)
    {
        if (!ModelState.IsValid || !YouTubeUrlHelper.TryGetVideoId(model.YouTubeUrl, out _))
        {
            TempData["Error"] = "Could not add video. Paste a valid YouTube link (watch, youtu.be, shorts, or embed).";
            return RedirectToAction(nameof(Index));
        }

        _context.VideoGuides.Add(new VideoGuide
        {
            Title = model.Title.Trim(),
            Description = model.Description?.Trim() ?? string.Empty,
            YouTubeUrl = model.YouTubeUrl.Trim(),
            Category = model.Category?.Trim() ?? string.Empty,
            IsPublished = model.IsPublished,
            SortOrder = model.SortOrder,
            DateAdded = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        TempData["Success"] = "Video guide added.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var guide = await _context.VideoGuides.FindAsync(id);
        if (guide == null)
        {
            TempData["Error"] = "Video guide not found.";
            return RedirectToAction(nameof(Index));
        }

        _context.VideoGuides.Remove(guide);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"Removed \"{guide.Title}\".";
        return RedirectToAction(nameof(Index));
    }
}
