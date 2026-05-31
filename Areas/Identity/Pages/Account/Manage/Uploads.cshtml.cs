using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Areas.Identity.Data;
using PindahWebsite3.Data;
using PindahWebsite3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace PindahWebsite3.Areas.Identity.Pages.Account.Manage
{
    public class UploadsModel : PageModel
    {
        private readonly PindahWebsite3Context _context;
        private readonly UserManager<PindahWebsite3User> _userManager;
        private readonly IWebHostEnvironment _env;

        public UploadsModel(PindahWebsite3Context context, UserManager<PindahWebsite3User> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        public IList<ZimsecCategory> Categories { get; set; } = new List<ZimsecCategory>();
        public IList<ZimsecDocument> MyDocuments { get; set; } = new List<ZimsecDocument>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Categories = await _context.ZimsecCategories.Include(c => c.ParentCategory).ToListAsync();
            MyDocuments = await _context.ZimsecDocuments
                .Include(d => d.Category)
                .Where(d => d.UploadedByUserId == userId)
                .OrderByDescending(d => d.UploadDate)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int CategoryId, string Title, IFormFile File)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (File != null && File.Length > 0 && CategoryId > 0 && !string.IsNullOrWhiteSpace(Title))
            {
                var category = await _context.ZimsecCategories.FindAsync(CategoryId);
                if (category != null)
                {
                    string safeCategoryName = string.Join("_", category.Name.Split(Path.GetInvalidFileNameChars()));
                    string uploadFolder = Path.Combine(_env.WebRootPath, "zimsec_documents", safeCategoryName);
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);
                    string dbPath = $"/zimsec_documents/{safeCategoryName}/{fileName}";

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await File.CopyToAsync(stream);
                    }

                    string extractedText = string.Empty;
                    if (Path.GetExtension(File.FileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
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

                            string textFilePath = Path.ChangeExtension(filePath, ".txt");
                            await System.IO.File.WriteAllTextAsync(textFilePath, extractedText);
                        }
                        catch { }
                    }

                    var documentRecord = new ZimsecDocument
                    {
                        Title = Title,
                        CategoryId = CategoryId,
                        FilePath = dbPath,
                        ExtractedText = extractedText,
                        UploadDate = DateTime.UtcNow,
                        UploadedByUserId = userId
                    };

                    _context.ZimsecDocuments.Add(documentRecord);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage();
        }
    }
}
