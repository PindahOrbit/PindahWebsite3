using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;

namespace PindahWebsite3.Controllers;

public class DownloadsController : Controller
{
    private readonly PindahWebsite3Context _context;

    public DownloadsController(PindahWebsite3Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var query = _context.Downloads.AsQueryable();

        if (User.Identity?.IsAuthenticated != true)
        {
            query = query.Where(d => d.IsPublished);
        }

        var downloads = await query
            .OrderBy(d => d.SortOrder)
            .ThenByDescending(d => d.DateAdded)
            .ToListAsync();

        ViewData["Title"] = "Downloads | Pindah Software & Mobile Apps";
        ViewData["Description"] = "Download Pindah mobile apps, installers, and software resources. Android APK and other releases maintained by Pindah Private Limited.";
        ViewData["Keywords"] = "Pindah downloads, mobile app, Android APK, software download, Zimbabwe enterprise software";

        return View(downloads);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DownloadSaveModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["DownloadError"] = "Could not add download. Check the form and try again.";
            return RedirectToAction(nameof(Index));
        }

        var download = new Download
        {
            Title = model.Title.Trim(),
            Description = model.Description?.Trim() ?? string.Empty,
            FileUrl = model.FileUrl.Trim(),
            FileType = InferFileType(model.FileType, model.FileUrl),
            Platform = model.Platform?.Trim() ?? string.Empty,
            IsPublished = model.IsPublished,
            SortOrder = model.SortOrder,
            DateAdded = DateTime.UtcNow
        };

        _context.Downloads.Add(download);
        await _context.SaveChangesAsync();

        TempData["DownloadSuccess"] = $"Added \"{download.Title}\".";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var download = await _context.Downloads.FindAsync(id);
        if (download == null)
        {
            TempData["DownloadError"] = "Download not found.";
            return RedirectToAction(nameof(Index));
        }

        _context.Downloads.Remove(download);
        await _context.SaveChangesAsync();

        TempData["DownloadSuccess"] = $"Removed \"{download.Title}\".";
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
