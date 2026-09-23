using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;

namespace PindahWebsite3.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = CmsConstants.RoleAdmin)]
public class DashboardController : Controller
{
    private readonly PindahWebsite3Context _context;

    public DashboardController(PindahWebsite3Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var published = await _context.News.CountAsync(n => n.Status == NewsStatus.Published);
        var drafts = await _context.News.CountAsync(n => n.Status == NewsStatus.Draft);
        var featured = await _context.News.CountAsync(n => n.IsFeatured);
        var downloads = await _context.Downloads.CountAsync();
        var users = await _context.Users.CountAsync();

        ViewBag.PublishedCount = published;
        ViewBag.DraftCount = drafts;
        ViewBag.FeaturedCount = featured;
        ViewBag.MaxFeatured = CmsConstants.MaxFeaturedSlots;
        ViewBag.DownloadCount = downloads;
        ViewBag.UserCount = users;

        var recent = await _context.News
            .AsNoTracking()
            .Include(n => n.Author)
            .OrderByDescending(n => n.DateModified ?? n.DateCreated)
            .Take(8)
            .ToListAsync();

        return View(recent);
    }
}
