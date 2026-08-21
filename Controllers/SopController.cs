using Markdig;
using Microsoft.AspNetCore.Mvc;

namespace PindahWebsite3.Controllers;

public class SopController : Controller
{
    private readonly IWebHostEnvironment _env;

    public SopController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [Route("/sop")]
    [Route("/home/sop")]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any)]
    public IActionResult Index()
    {
        var path = Path.Combine(_env.WebRootPath, "SOP.md");
        if (!System.IO.File.Exists(path))
        {
            return NotFound();
        }

        var markdown = System.IO.File.ReadAllText(path);
        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseAutoIdentifiers()
            .Build();

        var html = Markdown.ToHtml(markdown, pipeline);

        ViewData["Title"] = "Operations SOP — Standard Operating Procedure | Pindah Basa";
        ViewData["Description"] = "Complete standard operating procedure for Pindah Basa / Operations: registration, every module, workflows, testing, and troubleshooting from login to logout.";
        ViewData["Keywords"] = "Pindah Basa SOP, Operations manual, ERP procedures, pharmacy workflow, school management SOP, hospital management, accounting procedures Zimbabwe";
        ViewData["CanonicalUrl"] = "https://pindah.org/sop";

        return View(model: html);
    }
}
