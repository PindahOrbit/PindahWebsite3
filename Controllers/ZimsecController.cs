using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Services.Zimsec;
using PindahWebsite3.ViewModels;

namespace PindahWebsite3.Controllers;

public class ZimsecController : Controller
{
    private readonly ZimsecContext _zimsecDb;
    private readonly IZimsecCatalogService _catalog;
    private readonly IZimsecSearchService _search;
    private readonly ZimsecAuthService _auth;
    private readonly IZimsecLibraryIndexer _indexer;

    public ZimsecController(
        ZimsecContext zimsecDb,
        IZimsecCatalogService catalog,
        IZimsecSearchService search,
        ZimsecAuthService auth,
        IZimsecLibraryIndexer indexer)
    {
        _zimsecDb = zimsecDb;
        _catalog = catalog;
        _search = search;
        _auth = auth;
        _indexer = indexer;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        if (ZimsecAuthService.GetStudentId(User).HasValue)
            return RedirectToAction(nameof(Library));

        var tree = _catalog.GetTreeFromDisk();
        var model = new ZimsecIndexViewModel
        {
            PreviewTree = tree,
            TotalDocuments = tree.Sum(l => l.Subjects.Sum(s => s.DocumentCount)),
            TotalSubjects = tree.Sum(l => l.Subjects.Count)
        };

        if (TempData["RegisterSuccess"] is string regOk)
            model.RegisterSuccess = regOk;

        return View(model);
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string phoneNumber, string password)
    {
        var (ok, student, error) = await _auth.ValidateAsync(phoneNumber, password);
        if (!ok || student == null)
        {
            var tree = _catalog.GetTreeFromDisk();
            return View("Index", new ZimsecIndexViewModel
            {
                LoginError = error,
                PreviewTree = tree,
                TotalDocuments = tree.Sum(l => l.Subjects.Sum(s => s.DocumentCount)),
                TotalSubjects = tree.Sum(l => l.Subjects.Count)
            });
        }

        await _auth.SignInAsync(HttpContext, student);
        return RedirectToAction(nameof(Library));
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(string phoneNumber, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            var tree = _catalog.GetTreeFromDisk();
            return View("Index", new ZimsecIndexViewModel
            {
                RegisterError = "Passwords do not match.",
                PreviewTree = tree,
                TotalDocuments = tree.Sum(l => l.Subjects.Sum(s => s.DocumentCount)),
                TotalSubjects = tree.Sum(l => l.Subjects.Count)
            });
        }

        var (ok, error) = await _auth.RegisterAsync(phoneNumber, password);
        if (!ok)
        {
            var tree = _catalog.GetTreeFromDisk();
            return View("Index", new ZimsecIndexViewModel
            {
                RegisterError = error,
                PreviewTree = tree,
                TotalDocuments = tree.Sum(l => l.Subjects.Sum(s => s.DocumentCount)),
                TotalSubjects = tree.Sum(l => l.Subjects.Count)
            });
        }

        TempData["RegisterSuccess"] = "Account created. Sign in with your phone number.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(AuthenticationSchemes = ZimsecAuthDefaults.Scheme)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _auth.SignOutAsync(HttpContext);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(AuthenticationSchemes = ZimsecAuthDefaults.Scheme)]
    public async Task<IActionResult> Library(string? level, string? subject, string? q, CancellationToken cancellationToken)
    {
        level = NormalizeSlug(level);
        subject = NormalizeSlug(subject);
        q = q?.Trim();

        var tree = _catalog.GetTreeFromDisk();
        var searchResult = await _search.SearchAsync(q, level, subject, 50, cancellationToken);

        var browse = searchResult.Hits.Select(h => new ZimsecDocumentListItem(
            h.DocumentId, h.Title, h.FileName, h.Level, h.SubjectSlug, h.SubjectDisplay, 0, 0)).ToList();

        if (string.IsNullOrWhiteSpace(q))
        {
            var docQuery = _zimsecDb.Documents.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(level))
                docQuery = docQuery.Where(d => d.Level == level);
            if (!string.IsNullOrEmpty(subject))
                docQuery = docQuery.Where(d => d.SubjectSlug == subject);

            browse = await docQuery
                .OrderBy(d => d.Title)
                .Select(d => new ZimsecDocumentListItem(
                    d.Id, d.Title, d.FileName, d.Level, d.SubjectSlug, d.SubjectDisplay,
                    d.FileSizeBytes, d.PageCount))
                .Take(100)
                .ToListAsync(cancellationToken);
        }

        var model = new ZimsecLibraryViewModel
        {
            PhoneNumber = User.Identity?.Name ?? string.Empty,
            Tree = tree,
            SelectedLevel = level,
            SelectedSubject = subject,
            SelectedLevelDisplay = level != null ? _catalog.FormatLevelDisplay(level) : null,
            SelectedSubjectDisplay = subject != null ? _catalog.FormatSubjectDisplay(subject) : null,
            SearchQuery = q ?? string.Empty,
            SearchResult = searchResult,
            BrowseDocuments = browse
        };

        return View(model);
    }

    [Authorize(AuthenticationSchemes = ZimsecAuthDefaults.Scheme)]
    public async Task<IActionResult> ViewDocument(int id, string? level, string? subject, string? q)
    {
        var doc = await _zimsecDb.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        if (doc == null) return NotFound();

        var returnUrl = "/Zimsec/Library";
        var qs = new List<string>();
        if (!string.IsNullOrEmpty(level)) qs.Add($"level={Uri.EscapeDataString(level)}");
        if (!string.IsNullOrEmpty(subject)) qs.Add($"subject={Uri.EscapeDataString(subject)}");
        if (!string.IsNullOrEmpty(q)) qs.Add($"q={Uri.EscapeDataString(q)}");
        if (qs.Count > 0) returnUrl += "?" + string.Join("&", qs);

        return View(new ZimsecDocumentViewModel
        {
            Id = doc.Id,
            Title = doc.Title,
            Level = doc.Level,
            LevelDisplay = _catalog.FormatLevelDisplay(doc.Level),
            SubjectDisplay = doc.SubjectDisplay,
            SubjectSlug = doc.SubjectSlug,
            FileSizeBytes = doc.FileSizeBytes,
            PageCount = doc.PageCount,
            ReturnUrl = returnUrl
        });
    }

    [Authorize(AuthenticationSchemes = ZimsecAuthDefaults.Scheme)]
    [HttpGet]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new[] { "id" })]
    public async Task<IActionResult> StreamPdf(int id)
    {
        var doc = await _zimsecDb.Documents.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        if (doc == null) return NotFound();

        var physicalPath = _catalog.ResolvePhysicalPath(doc.RelativePath);
        if (physicalPath == null) return NotFound();

        var stream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        Response.Headers["Content-Disposition"] = $"inline; filename=\"{Uri.EscapeDataString(doc.FileName)}\"";
        return File(stream, "application/pdf");
    }

    [Authorize(AuthenticationSchemes = ZimsecAuthDefaults.Scheme)]
    [HttpGet]
    public async Task<IActionResult> SearchApi(string? q, string? level, string? subject, CancellationToken cancellationToken)
    {
        var result = await _search.SearchAsync(q, NormalizeSlug(level), NormalizeSlug(subject), 20, cancellationToken);
        return Json(new
        {
            query = result.Query,
            total = result.TotalCount,
            hits = result.Hits.Select(h => new
            {
                id = h.DocumentId,
                title = h.Title,
                level = h.LevelDisplay,
                subject = h.SubjectDisplay,
                snippet = h.Snippet,
                score = h.Score,
                url = Url.Action(nameof(ViewDocument), new { id = h.DocumentId, level, subject, q })
            }),
            facets = new
            {
                levels = result.LevelFacets,
                subjects = result.SubjectFacets
            }
        });
    }

    private static string? NormalizeSlug(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        return value.Trim().ToLowerInvariant();
    }
}
