using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Models;

namespace PindahWebsite3.Controllers;

public class HomeController : Controller
{
    private readonly PindahWebsite3Context _context;

    public HomeController(PindahWebsite3Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeIndexViewModel
        {
            FeaturedNews = await _context.News
                .OrderByDescending(n => n.DateCreated)
                .Take(3)
                .ToListAsync(),
            Modules = GetModuleCards()
        };

        return View(model);
    }

    private static List<ModuleCardViewModel> GetModuleCards() =>
    [
        new ModuleCardViewModel
        {
            IconClass = "bi-diagram-3",
            Title = "Enterprise Resource Planning",
            Description = "Centralized finance, procurement, inventory, sales, and project management. Native multi-currency support for Zimbabwe's USD/ZiG environment with IFRS compliance at transaction level.",
            LinkText = "ERP solutions",
            Controller = "Erp"
        },
        new ModuleCardViewModel
        {
            IconClass = "bi-calculator",
            Title = "Accounting & Financial Management",
            Description = "General ledger, accounts payable, accounts receivable, fixed assets, and tax management. Automated ZIMRA fiscal device integration and statutory reporting.",
            LinkText = "Accounting solutions",
            Controller = "Accounting"
        },
        new ModuleCardViewModel
        {
            IconClass = "bi-people",
            Title = "Customer Relationship Management",
            Description = "Complete sales pipeline, lead tracking, quotation management, and service desk. Unified customer history across every touchpoint.",
            LinkText = "CRM solutions",
            Controller = "Crm"
        },
        new ModuleCardViewModel
        {
            IconClass = "bi-hospital",
            Title = "Hospital & Healthcare Management",
            Description = "Patient registration, clinical records, outpatient and inpatient workflows, pharmacy, laboratory, and healthcare billing integrated end-to-end.",
            LinkText = "Healthcare solutions",
            Controller = "Hospital"
        },
        new ModuleCardViewModel
        {
            IconClass = "bi-mortarboard",
            Title = "School Management System",
            Description = "Student enrollment, academic records, attendance, fee billing, timetabling, and parent communication. Complete student lifecycle administration.",
            LinkText = "Education solutions",
            Controller = "Sms"
        },
        new ModuleCardViewModel
        {
            IconClass = "bi-book",
            Title = "Pindah Course",
            Description = "Free Heritage-Based Curriculum lessons for Zimbabwe primary schools — grade courses, stories, practice, and teacher-guide PDFs. A Pindah.org product beside Frame and Basa.",
            LinkText = "Open Pindah Course",
            Href = "https://courses.edtech.co.zw"
        },
        new ModuleCardViewModel
        {
            IconClass = "bi-gear-wide-connected",
            Title = "Manufacturing & Production",
            Description = "Bill of materials, production scheduling, shop floor control, quality management, costing, and maintenance. ISO-aligned manufacturing execution.",
            LinkText = "Manufacturing solutions",
            Controller = "Manufacturing"
        },
        new ModuleCardViewModel
        {
            IconClass = "bi-truck",
            Title = "Logistics & Fleet Management",
            Description = "Route optimization, vehicle tracking, driver management, cross-border documentation, and delivery analytics for transport operators.",
            LinkText = "Logistics solutions",
            Controller = "Logistics"
        },
        new ModuleCardViewModel
        {
            IconClass = "bi-shield-plus",
            Title = "Insurance & Broker Management",
            Description = "Policy administration, underwriting, claims processing, premium collection, and broker management for insurance operations.",
            LinkText = "Insurance solutions",
            Controller = "Insurance"
        },
        new ModuleCardViewModel
        {
            IconClass = "bi-bricks",
            Title = "Construction & Project Control",
            Description = "Project planning, cost control, progress billing, site management, and subcontractor coordination for construction firms.",
            LinkText = "Construction solutions",
            Controller = "Construction"
        }
    ];

    [Route("/privacy")]
    [Route("/home/privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
