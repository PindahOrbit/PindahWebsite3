using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;
using PindahWebsite3.ViewModels;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace PindahWebsite3.Controllers
{
    public class ZimsecController : Controller
    {
        private readonly PindahWebsite3Context _context;
        private readonly IWebHostEnvironment _env;

        public ZimsecController(PindahWebsite3Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(string q = null)
        {
            var flatCategories = await _context.ZimsecCategories.ToListAsync();

            var viewModel = new ZimsecIndexViewModel
            {
                FlatCategories = flatCategories,
                SearchQuery = q ?? string.Empty
            };

            if (!string.IsNullOrWhiteSpace(q))
            {
                viewModel.IsSearch = true;
                string queryStr = q.Trim().ToLower();

                // Advanced network search parser (e.g. "Maths > Vectors")
                var queryParts = queryStr.Split('>').Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
                
                string mainQuery = queryParts.LastOrDefault() ?? queryStr;
                string parentQuery = queryParts.Count > 1 ? queryParts.First() : null;

                // Categories query
                var catsQuery = _context.ZimsecCategories.Include(c => c.ParentCategory).AsQueryable();
                if (parentQuery != null)
                {
                    catsQuery = catsQuery.Where(c => c.Name.ToLower().Contains(mainQuery) && c.ParentCategory != null && c.ParentCategory.Name.ToLower().Contains(parentQuery));
                }
                else
                {
                    catsQuery = catsQuery.Where(c => c.Name.ToLower().Contains(mainQuery));
                }
                viewModel.SearchCategoryResults = await catsQuery.ToListAsync();

                // Documents query
                var docsQuery = _context.ZimsecDocuments.Include(d => d.Category).ThenInclude(c => c.ParentCategory).AsQueryable();
                if (parentQuery != null)
                {
                    docsQuery = docsQuery.Where(d => 
                        (d.Title.ToLower().Contains(mainQuery) || d.ExtractedText.ToLower().Contains(mainQuery) || (d.Category != null && d.Category.Name.ToLower().Contains(mainQuery))) && 
                        (d.Category != null && d.Category.ParentCategory != null && d.Category.ParentCategory.Name.ToLower().Contains(parentQuery)));
                }
                else
                {
                    docsQuery = docsQuery.Where(d => 
                        d.Title.ToLower().Contains(mainQuery) || 
                        (d.Category != null && d.Category.Name.ToLower().Contains(mainQuery)) ||
                        d.ExtractedText.ToLower().Contains(mainQuery));
                }
                viewModel.SearchDocumentResults = await docsQuery.OrderByDescending(d => d.UploadDate).Take(100).ToListAsync();
            }
            else
            {
                viewModel.Categories = await _context.ZimsecCategories
                    .Include(c => c.SubCategories)
                    .Where(c => c.ParentCategoryId == null)
                    .ToListAsync();

                viewModel.RecentDocuments = await _context.ZimsecDocuments
                    .Include(d => d.Category)
                    .OrderByDescending(d => d.UploadDate)
                    .Take(20)
                    .ToListAsync();
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(string name, int? parentCategoryId)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var category = new ZimsecCategory
                {
                    Name = name.Trim(),
                    ParentCategoryId = (parentCategoryId > 0) ? parentCategoryId : null
                };
                _context.ZimsecCategories.Add(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocuments(int categoryId, List<IFormFile> files)
        {
            if (files != null && files.Any() && categoryId > 0)
            {
                var category = await _context.ZimsecCategories.FindAsync(categoryId);
                if (category == null) return RedirectToAction(nameof(Index));

                // Create folder based on category structure or just a general one appropriately inside wwwroot
                string safeCategoryName = string.Join("_", category.Name.Split(Path.GetInvalidFileNameChars()));
                string uploadFolder = Path.Combine(_env.WebRootPath, "zimsec_documents", safeCategoryName);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                foreach (var file in files)
                {
                    if (file.Length == 0) continue;

                    string title = Path.GetFileNameWithoutExtension(file.FileName);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);
                    string dbPath = $"/zimsec_documents/{safeCategoryName}/{fileName}";

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Extract text if PDF and save next to the document
                    string extractedText = string.Empty;
                    if (Path.GetExtension(file.FileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            using (var document = PdfDocument.Open(filePath))
                            {
                                StringBuilder textBuilder = new StringBuilder();
                                foreach (var page in document.GetPages())
                                {
                                    textBuilder.AppendLine(page.Text);
                                }
                                extractedText = textBuilder.ToString();
                            }

                            // Save text to log file next to PDF
                            string textFilePath = Path.ChangeExtension(filePath, ".txt");
                            await System.IO.File.WriteAllTextAsync(textFilePath, extractedText);
                        }
                        catch
                        {
                            // Safely handle if doc is malformed and still add to system but skip text.
                        }
                    }

                    var documentRecord = new ZimsecDocument
                    {
                        Title = title,
                        CategoryId = categoryId,
                        FilePath = dbPath,
                        ExtractedText = extractedText,
                        UploadDate = DateTime.UtcNow,
                        UploadedByUserId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)
                    };

                    _context.ZimsecDocuments.Add(documentRecord);
                }
                
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.ZimsecCategories.FindAsync(id);
            if (category != null)
            {
                var documents = await _context.ZimsecDocuments.Where(d => d.CategoryId == id).ToListAsync();
                foreach (var doc in documents)
                {
                    doc.CategoryId = null;
                }

                var subCategories = await _context.ZimsecCategories.Where(c => c.ParentCategoryId == id).ToListAsync();
                foreach (var sub in subCategories)
                {
                    sub.ParentCategoryId = null;
                }

                _context.ZimsecCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ViewDocument(int id)
        {
            var doc = await _context.ZimsecDocuments.Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == id);
            if (doc == null)
            {
                return NotFound();
            }
            return View(doc);
        }
        [HttpGet]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> StreamPdf(int id)
        {
            var doc = await _context.ZimsecDocuments.FindAsync(id);
            if (doc == null) return NotFound();

            var physicalPath = Path.Combine(_env.WebRootPath, doc.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (!System.IO.File.Exists(physicalPath)) return NotFound();

            var stream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Response.Headers["Content-Disposition"] = "inline; filename=\"" + Uri.EscapeDataString(doc.Title) + ".pdf\"";
            return File(stream, "application/pdf");
        }
    }
}
