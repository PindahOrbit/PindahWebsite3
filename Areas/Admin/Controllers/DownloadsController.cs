using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;

namespace PindahWebsite3.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = CmsConstants.RoleAdmin)]
public class DownloadsController : Controller
{
    private readonly PindahWebsite3Context _context;

    public DownloadsController(PindahWebsite3Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var downloads = await _context.Downloads
            .OrderBy(d => d.SortOrder)
            .ThenByDescending(d => d.DateAdded)
            .ToListAsync();
        return View(downloads);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DownloadSaveModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Could not add download. Check the form and try again.";
            return RedirectToAction(nameof(Index));
        }

        _context.Downloads.Add(new Download
        {
            Title = model.Title.Trim(),
            Description = model.Description?.Trim() ?? string.Empty,
            FileUrl = model.FileUrl.Trim(),
            FileType = InferFileType(model.FileType, model.FileUrl),
            Platform = model.Platform?.Trim() ?? string.Empty,
            IsPublished = model.IsPublished,
            SortOrder = model.SortOrder,
            DateAdded = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        TempData["Success"] = "Download added.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var download = await _context.Downloads.FindAsync(id);
        if (download == null)
        {
            TempData["Error"] = "Download not found.";
            return RedirectToAction(nameof(Index));
        }

        _context.Downloads.Remove(download);
        await _context.SaveChangesAsync();
        TempData["Success"] = $"Removed \"{download.Title}\".";
        return RedirectToAction(nameof(Index));
    }

    private static string InferFileType(string fileType, string fileUrl)
    {
        if (!string.IsNullOrWhiteSpace(fileType))
        {
            return fileType.Trim().ToUpperInvariant();
        }

        var extension = Path.GetExtension(fileUrl);
        return string.IsNullOrWhiteSpace(extension)
            ? string.Empty
            : extension.TrimStart('.').ToUpperInvariant();
    }
}
