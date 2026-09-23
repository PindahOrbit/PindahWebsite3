using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;
using PindahWebsite3.Services;

namespace PindahWebsite3.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{CmsConstants.RoleAdmin},{CmsConstants.RoleContributor}")]
public class ArticlesController : Controller
{
    private readonly PindahWebsite3Context _context;
    private readonly FeaturedNewsService _featuredNews;

    public ArticlesController(PindahWebsite3Context context, FeaturedNewsService featuredNews)
    {
        _context = context;
        _featuredNews = featuredNews;
    }

    public async Task<IActionResult> Index(string? status)
    {
        var isAdmin = User.IsInRole(CmsConstants.RoleAdmin);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var query = _context.News.AsNoTracking().Include(n => n.Author).AsQueryable();

        if (!isAdmin)
        {
            query = query.Where(n => n.AuthorId == userId);
        }

        if (Enum.TryParse<NewsStatus>(status, true, out var statusFilter))
        {
            query = query.Where(n => n.Status == statusFilter);
        }

        var articles = await query
            .OrderByDescending(n => n.DateModified ?? n.DateCreated)
            .ToListAsync();

        ViewBag.IsAdmin = isAdmin;
        ViewBag.StatusFilter = status;
        ViewBag.FeaturedCount = await _context.News.CountAsync(n => n.IsFeatured);
        ViewBag.MaxFeatured = CmsConstants.MaxFeaturedSlots;
        return View(articles);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.IsAdmin = User.IsInRole(CmsConstants.RoleAdmin);
        return View("Edit", new NewsSaveModel { Status = NewsStatus.Draft });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var article = await _context.News.FindAsync(id);
        if (article == null)
        {
            return NotFound();
        }

        if (!CanManage(article))
        {
            return Forbid();
        }

        ViewBag.IsAdmin = User.IsInRole(CmsConstants.RoleAdmin);
        return View(new NewsSaveModel
        {
            Id = article.Id,
            Heading = article.Heading,
            Content = article.Content,
            CoverImageUrl = article.CoverImageUrl,
            Slug = article.Slug,
            Status = article.Status,
            IsFeatured = article.IsFeatured
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(NewsSaveModel model, CancellationToken cancellationToken)
    {
        ViewBag.IsAdmin = User.IsInRole(CmsConstants.RoleAdmin);

        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Challenge();
        }

        var isAdmin = User.IsInRole(CmsConstants.RoleAdmin);
        if (!isAdmin && model.IsFeatured)
        {
            model.IsFeatured = false;
        }

        News? article;
        if (model.Id is int id)
        {
            article = await _context.News.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
            if (article is null)
            {
                return NotFound();
            }

            if (!CanManage(article))
            {
                return Forbid();
            }

            article.Heading = model.Heading.Trim();
            article.Content = model.Content.Trim();
            article.CoverImageUrl = model.CoverImageUrl.Trim();
            article.DateModified = DateTime.UtcNow;
            ApplyStatus(article, model.Status);
            await ApplySlugAsync(article, model.Slug, model.Heading, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            article = new News
            {
                Heading = model.Heading.Trim(),
                Content = model.Content.Trim(),
                CoverImageUrl = model.CoverImageUrl.Trim(),
                AuthorId = userId,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };
            ApplyStatus(article, model.Status);
            await ApplySlugAsync(article, model.Slug, model.Heading, cancellationToken);
            _context.News.Add(article);
            await _context.SaveChangesAsync(cancellationToken);
        }

        if (isAdmin)
        {
            var featureResult = await _featuredNews.SetFeaturedAsync(article.Id, model.IsFeatured && article.Status == NewsStatus.Published, cancellationToken);
            if (!featureResult.Ok && model.IsFeatured)
            {
                TempData["Error"] = featureResult.Message;
                return RedirectToAction(nameof(Edit), new { id = article.Id });
            }
        }

        TempData["Success"] = "Article saved.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var article = await _context.News.FindAsync([id], cancellationToken);
        if (article == null)
        {
            return NotFound();
        }

        if (!CanManage(article))
        {
            return Forbid();
        }

        _context.News.Remove(article);
        await _context.SaveChangesAsync(cancellationToken);
        await _featuredNews.CompactRanksAsync(cancellationToken);
        TempData["Success"] = "Article deleted.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = CmsConstants.RoleAdmin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleFeatured(int id, CancellationToken cancellationToken)
    {
        var article = await _context.News.FindAsync([id], cancellationToken);
        if (article == null)
        {
            return NotFound();
        }

        var result = await _featuredNews.SetFeaturedAsync(id, !article.IsFeatured, cancellationToken);
        TempData[result.Ok ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    private bool CanManage(News article)
    {
        if (User.IsInRole(CmsConstants.RoleAdmin))
        {
            return true;
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return article.AuthorId == userId;
    }

    private static void ApplyStatus(News article, NewsStatus status)
    {
        article.Status = status;
        if (status == NewsStatus.Published && article.DatePublished == null)
        {
            article.DatePublished = DateTime.UtcNow;
        }

        if (status != NewsStatus.Published)
        {
            article.IsFeatured = false;
            article.FeaturedRank = null;
        }
    }

    private async Task ApplySlugAsync(News article, string? requestedSlug, string heading, CancellationToken cancellationToken)
    {
        var baseSlug = string.IsNullOrWhiteSpace(requestedSlug)
            ? GenerateSlug(heading)
            : GenerateSlug(requestedSlug);

        // Keep existing slug on edit when request matches current (avoid hash churn).
        if (article.Id > 0 && !string.IsNullOrWhiteSpace(article.Slug)
            && (string.IsNullOrWhiteSpace(requestedSlug) || GenerateSlugBase(requestedSlug) == GenerateSlugBase(article.Slug)))
        {
            if (string.IsNullOrWhiteSpace(requestedSlug))
            {
                return;
            }
        }

        var slug = baseSlug;
        var counter = 1;
        while (await _context.News.AnyAsync(n => n.Slug == slug && n.Id != article.Id, cancellationToken))
        {
            slug = $"{GenerateSlugBase(baseSlug)}-{counter}";
            counter++;
        }

        article.Slug = slug;
    }

    private static string GenerateSlug(string input)
    {
        var baseSlug = GenerateSlugBase(input);
        var hash = Guid.NewGuid().ToString("N")[..6];
        return $"{baseSlug}-{hash}";
    }

    private static string GenerateSlugBase(string input)
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

        return string.IsNullOrEmpty(slug) ? "article" : slug;
    }
}
