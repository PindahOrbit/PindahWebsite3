using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Data;
using PindahWebsite3.Services;
using System.Xml.Linq;

namespace PindahWebsite3.Controllers;

public class SitemapController : Controller
{
    private static readonly XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
    private readonly PindahWebsite3Context _context;

    public SitemapController(PindahWebsite3Context context)
    {
        _context = context;
    }

    [Route("sitemap.xml")]
    public async Task<IActionResult> Index()
    {
        var baseUrl = "https://pindah.org";
        var now = DateTime.UtcNow;

        var urls = new List<XElement>
        {
            CreateUrlEntry($"{baseUrl}", now, "daily", "1.0"),
            CreateUrlEntry($"{baseUrl}/privacy", now.AddDays(-7), "monthly", "0.3"),
            CreateUrlEntry($"{baseUrl}/sop", now, "weekly", "0.6"),
            CreateUrlEntry($"{baseUrl}/news", now.AddDays(-1), "daily", "0.8"),
            CreateUrlEntry($"{baseUrl}/crm", now.AddDays(-1), "weekly", "0.9"),
            CreateUrlEntry($"{baseUrl}/crm/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/leads", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/opportunities", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/pipeline", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/accounts", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/contacts", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/customers", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/activities", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/cases", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/inbox", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/campaigns", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/cadences", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/forecasts", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/quotes", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/knowledge", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/pipelinestages", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/leadsources", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/crm/reports", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp", now.AddDays(-1), "weekly", "1.0"),
            CreateUrlEntry($"{baseUrl}/erp/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/generalledger", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/accountspayable", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/accountsreceivable", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/inventory", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/procurement", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/pointofsale", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/salesinvoicing", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/projectmanagement", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/fixedassets", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/budgeting", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/reporting", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/erp/audit", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/construction", now.AddDays(-1), "weekly", "0.8"),
            CreateUrlEntry($"{baseUrl}/construction/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/construction/tendering", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/construction/contracts", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/construction/projectplanning", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/construction/sitemanagement", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/construction/subcontractors", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/construction/costcontrol", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/construction/progressbilling", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/construction/safetyhealth", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hr", now.AddDays(-1), "weekly", "0.9"),
            CreateUrlEntry($"{baseUrl}/hr/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hr/recruitment", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hr/onboarding", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hr/records", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hr/leave", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hr/payroll", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hr/performance", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hr/training", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hr/analytics", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hospital", now.AddDays(-1), "weekly", "0.9"),
            CreateUrlEntry($"{baseUrl}/hospital/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hospital/registration", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hospital/outpatient", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hospital/inpatient", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hospital/medicalrecords", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hospital/laboratory", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hospital/pharmacy", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hospital/radiology", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/hospital/billing", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/basarx", now.AddDays(-1), "weekly", "0.9"),
            CreateUrlEntry($"{baseUrl}/basarx/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/basarx/dispensing", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/basarx/ehr", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/basarx/inventory", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/basarx/refills", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/basarx/claims", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/basarx/patients", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/basarx/integration", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/manufacturing", now.AddDays(-1), "weekly", "0.9"),
            CreateUrlEntry($"{baseUrl}/manufacturing/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/manufacturing/billofmaterials", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/manufacturing/planning", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/manufacturing/shopfloor", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/manufacturing/qualitycontrol", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/manufacturing/traceability", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/manufacturing/costing", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/manufacturing/maintenance", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/insurance", now.AddDays(-1), "weekly", "0.9"),
            CreateUrlEntry($"{baseUrl}/insurance/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/insurance/policyadministration", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/insurance/underwriting", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/insurance/claims", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/insurance/reinsurance", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/insurance/premiumcollection", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/insurance/brokermanagement", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/insurance/regulatory", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/logistics", now.AddDays(-1), "weekly", "0.9"),
            CreateUrlEntry($"{baseUrl}/logistics/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/logistics/drivers", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/logistics/vehicles", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/logistics/clients", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/logistics/trips", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/logistics/commandcenter", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/logistics/routeoptimization", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/logistics/proofofdelivery", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/logistics/analytics", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting", now.AddDays(-1), "weekly", "1.0"),
            CreateUrlEntry($"{baseUrl}/accounting/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting/journalentry", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting/issuetransfer", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting/invoices", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting/quotations", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting/purchaseorders", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting/customers", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting/paymentmethods", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting/currencies", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting/feestaxes", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/accounting/banks", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/dms", now.AddDays(-1), "weekly", "1.0"),
            CreateUrlEntry($"{baseUrl}/dms/capture", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/dms/classification", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/dms/versioncontrol", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/dms/workflow", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/dms/search", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/dms/security", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/dms/retention", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/dms/collaboration", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/dms/backgroundprocessing", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/scm", now.AddDays(-1), "weekly", "0.8"),
            CreateUrlEntry($"{baseUrl}/sms", now.AddDays(-1), "weekly", "0.9"),
            CreateUrlEntry($"{baseUrl}/sms/dashboard", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/sms/admissions", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/sms/attendance", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/sms/academics", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/sms/examinations", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/sms/fees", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/sms/library", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/sms/staff", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/sms/portals", now.AddDays(-1), "weekly", "0.7"),
            CreateUrlEntry($"{baseUrl}/sms/boarding", now.AddDays(-1), "weekly", "0.7")
        };

        urls.AddRange(SeoLandingCatalog.All.Select(page =>
            CreateUrlEntry($"{baseUrl}/{page.Slug}", now.AddDays(-1), "weekly", "0.85")));

        var articles = await _context.News
            .AsNoTracking()
            .Select(n => new { n.Slug, n.DateCreated })
            .ToListAsync();

        urls.AddRange(articles.Select(a =>
            CreateUrlEntry($"{baseUrl}/news/details/{a.Slug}", a.DateCreated, "monthly", "0.7")));

        var sitemap = new XElement(ns + "urlset",
            new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
            urls
        );

        return Content(sitemap.ToString(), "application/xml");
    }

    private static XElement CreateUrlEntry(string loc, DateTime lastmod, string changefreq, string priority)
    {
        return new XElement(ns + "url",
            new XElement(ns + "loc", loc),
            new XElement(ns + "lastmod", lastmod.ToString("yyyy-MM-dd")),
            new XElement(ns + "changefreq", changefreq),
            new XElement(ns + "priority", priority)
        );
    }
}
