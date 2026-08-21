using PindahWebsite3.Models;

namespace PindahWebsite3.Services;

public static class SeoLandingCatalog
{
  private static readonly Dictionary<string, SeoLandingPage> Pages = BuildPages()
      .ToDictionary(p => p.Slug, StringComparer.OrdinalIgnoreCase);

  public static IReadOnlyCollection<SeoLandingPage> All => Pages.Values;

  public static bool TryGet(string slug, out SeoLandingPage page) =>
      Pages.TryGetValue(slug, out page!);

  public static SeoLandingPage Get(string slug) =>
      Pages.TryGetValue(slug, out var page) ? page : throw new KeyNotFoundException(slug);

  private static SeoLandingPage Page(
      string slug,
      string title,
      string description,
      string keywords,
      string h1,
      string kicker,
      string lead,
      string moduleUrl,
      string moduleLabel,
      params string[] benefits) => new()
  {
    Slug = slug,
    Title = title,
    Description = description,
    Keywords = keywords,
    H1 = h1,
    Kicker = kicker,
    Lead = lead,
    ModuleUrl = moduleUrl,
    ModuleLabel = moduleLabel,
    Benefits = benefits,
    Faqs = DefaultFaqs(h1, moduleLabel)
  };

  private static IReadOnlyList<SeoFaqItem> DefaultFaqs(string topic, string module) =>
  [
    new()
    {
      Question = $"How much does {topic.ToLowerInvariant()} cost in Zimbabwe?",
      Answer = "Pindah pricing depends on modules, users, and deployment scope. Contact us for a tailored quote — initial discussions are free and focused on your operational requirements."
    },
    new()
    {
      Question = "Can the system handle USD and ZiG?",
      Answer = "Yes. Pindah platforms support native multi-currency operations for Zimbabwe's USD and ZiG environment, including reporting and compliance workflows."
    },
    new()
    {
      Question = "Do you provide implementation and training in Harare?",
      Answer = "Yes. Pindah delivers structured discovery, implementation, data migration, training, and ongoing support for organizations across Zimbabwe."
    },
    new()
    {
      Question = $"Is {module} suitable for growing organizations?",
      Answer = "Pindah is built for organizations that have outgrown spreadsheets and disconnected tools — with scalable architecture, role-based access, and integrated reporting."
    }
  ];

  private static IEnumerable<SeoLandingPage> BuildPages()
  {
    yield return Page(
        "school-management-software-zimbabwe",
        "School Management Software Zimbabwe | Best School ERP | Pindah Frame",
        "Best school management software in Zimbabwe. Student enrollment, fees in USD/ZiG, ZIMSEC grading, attendance, parent portals, and school ERP for private and public schools in Harare and nationwide.",
        "school management software Zimbabwe, school management system Zimbabwe, student management system Zimbabwe, school ERP Zimbabwe, best school management software Zimbabwe, online school management system Zimbabwe, student information system Zimbabwe, school fees management software Zimbabwe, ERP for schools Zimbabwe, Pindah Frame",
        "School Management Software Zimbabwe",
        "Education software · Harare & nationwide",
        "Frame by Pindah is school management software built for Zimbabwean schools — from enrollment and ZIMSEC-aligned academics to fee billing in USD and ZiG and parent communication on WhatsApp.",
        "/sms",
        "Explore Frame SMS",
        "Student enrollment and admissions workflows",
        "School fees, invoicing, and arrears in USD/ZiG",
        "ZIMSEC grading, report cards, and examinations",
        "Teacher, parent, and student portals",
        "Attendance, boarding, and staff administration");

    yield return Page(
        "school-software-zimbabwe",
        "School Software Zimbabwe | Private & Public Schools | Pindah",
        "School software for Zimbabwe — manage students, fees, academics, and administration in one system. Trusted approach for private schools, high schools, and primary schools.",
        "school software Zimbabwe, private school software Zimbabwe, primary school management software Zimbabwe, high school management software Zimbabwe, school administration software Harare",
        "School Software Zimbabwe",
        "Frame · School administration",
        "Whether you run a private school in Harare or a regional high school, Pindah Frame digitizes the full student lifecycle with Zimbabwe-specific fee and academic workflows.",
        "/sms",
        "View school management module",
        "Private and public school support",
        "Primary and high school administration",
        "Digital report cards and exam scheduling",
        "Fee collection reconciliation for EcoCash and bank",
        "Centralized student records");

    yield return Page(
        "accounting-software-zimbabwe",
        "Accounting Software Zimbabwe | ZIMRA Compliant | Best Accounting System",
        "Best accounting software in Zimbabwe. ZIMRA-compliant invoicing, VAT, multi-currency USD/ZiG ledgers, bank reconciliation, and IFRS financial reporting for Harare businesses.",
        "accounting software Zimbabwe, Zimbabwe accounting software, best accounting software Zimbabwe, ZIMRA compliant accounting software, tax compliant accounting software Zimbabwe, accounting system Zimbabwe, financial management software Zimbabwe, invoice software Zimbabwe, billing software Zimbabwe",
        "Accounting Software Zimbabwe",
        "Finance · ZIMRA & IFRS aligned",
        "Pindah Accounting gives Zimbabwean finance teams a single system for general ledger, invoicing, VAT, withholding tax, and multi-currency books — without month-end reconciliation nightmares.",
        "/accounting",
        "Explore accounting module",
        "General ledger and chart of accounts",
        "ZIMRA fiscal device and VAT workflows",
        "USD and ZiG multi-currency posting",
        "Accounts payable, receivable, and banking",
        "Management accounts and IFRS reporting");

    yield return Page(
        "accounting-system-zimbabwe",
        "Accounting System Zimbabwe | Multi-Currency Finance Software | Pindah",
        "Complete accounting system for Zimbabwe organizations. Invoicing, stock-linked costing, payroll integration, and real-time financial visibility in Harare and beyond.",
        "accounting system Zimbabwe, invoice system Zimbabwe, invoice software Zimbabwe, billing software Zimbabwe, financial management software Zimbabwe",
        "Accounting System Zimbabwe",
        "Integrated finance platform",
        "Replace fragmented spreadsheets with an accounting system that connects sales, inventory, procurement, and payroll into one auditable financial record.",
        "/accounting",
        "See accounting features",
        "Quote-to-invoice automation",
        "Multi-entity consolidation",
        "Bank and mobile money reconciliation",
        "Budget and cost centre tracking",
        "Immutable audit trails");

    yield return Page(
        "erp-software-zimbabwe",
        "ERP Software Zimbabwe | Best ERP System | Pindah",
        "Best ERP software in Zimbabwe. Integrated finance, inventory, procurement, sales, HR, and industry modules for organizations in Harare and across the country.",
        "ERP Zimbabwe, ERP software Zimbabwe, Zimbabwe ERP, best ERP Zimbabwe, business management software Zimbabwe, manufacturing ERP Zimbabwe, retail ERP Zimbabwe, wholesale ERP Zimbabwe, ERP Harare, Pindah ERP",
        "ERP Software Zimbabwe",
        "Enterprise platform · Pindah",
        "Pindah ERP is the operating platform for Zimbabwean organizations that need one source of truth across finance, operations, and industry workflows — IFRS compliant and built for local infrastructure.",
        "/erp",
        "Explore Pindah ERP",
        "Unified finance and operations",
        "Inventory, procurement, and sales",
        "Project and fixed asset management",
        "Real-time dashboards and IFRS reporting",
        "Offline-capable deployment options");

    yield return Page(
        "erp-harare",
        "ERP Harare | Enterprise Software Company Zimbabwe | Pindah",
        "ERP solutions in Harare for growing businesses. Local implementation, training, and support for integrated finance, stock, CRM, and industry-specific modules.",
        "ERP Harare, enterprise software Harare, software company Zimbabwe, software development company Harare, software developers Zimbabwe, custom software development Zimbabwe",
        "ERP Harare",
        "Harare · Implementation & support",
        "Pindah is a Zimbabwe software company headquartered in Harare, delivering ERP and custom enterprise systems with on-the-ground discovery, rollout, and optimization.",
        "/erp",
        "Request ERP demo",
        "Harare-based discovery and support",
        "Custom workflows for your industry",
        "Phased implementation and training",
        "Integration with existing tools",
        "Scalable from SME to enterprise");

    yield return Page(
        "hospital-management-system-zimbabwe",
        "Hospital Management System Zimbabwe | Best HIMS | Pindah",
        "Best hospital management system in Zimbabwe. Patient registration, EMR, pharmacy, laboratory, billing, and medical aid integration for hospitals and clinics.",
        "hospital management system Zimbabwe, hospital information system Zimbabwe, best hospital management system Zimbabwe, healthcare software Zimbabwe, clinic management software Zimbabwe, medical software Zimbabwe, ERP for pharmacies Zimbabwe",
        "Hospital Management System Zimbabwe",
        "Healthcare · Hospitals & clinics",
        "Pindah Health digitizes the patient journey for Zimbabwean facilities — OPD, inpatient, pharmacy, lab, radiology, and medical aid billing with CIMAS and PSMAS workflows.",
        "/hospital",
        "Explore healthcare module",
        "Master patient index and registration",
        "Electronic medical records (EMR)",
        "Pharmacy and laboratory modules",
        "Medical aid claims and billing",
        "Clinical dashboards and reporting");

    yield return Page(
        "clinic-software-zimbabwe",
        "Clinic Software Zimbabwe | OPD & Patient Management | Pindah",
        "Clinic management software for Zimbabwe. Queue management, consultations, prescribing, billing, and patient records for private clinics and specialist practices in Harare.",
        "clinic software Zimbabwe, clinic management software Zimbabwe, clinic management software Harare, medical software Zimbabwe, outpatient management Zimbabwe",
        "Clinic Software Zimbabwe",
        "Clinics & specialist practices",
        "Run your clinic on one platform — from front-desk registration and triage to digital prescriptions and same-day medical aid reconciliation.",
        "/hospital",
        "View clinic workflows",
        "OPD queue and triage",
        "SOAP notes and e-prescribing",
        "Pharmacy stock linked to dispensing",
        "Medical aid verification",
        "Daily cash and claims reporting");

    yield return Page(
        "pharmacy-management-software-zimbabwe",
        "Pharmacy Management Software Zimbabwe | Hospital & Retail Pharmacy",
        "Pharmacy management software for Zimbabwe. Stock control, expiry tracking, dispensing, billing, and integration with hospital and clinic workflows.",
        "pharmacy management software Zimbabwe, pharmacy software Zimbabwe, ERP for pharmacies Zimbabwe, clinic pharmacy software Harare",
        "Pharmacy Management Software Zimbabwe",
        "Pharmacy · Stock & dispensing",
        "Manage formulary, batches, expiry dates, and dispensing with pharmacy software integrated into Pindah's wider healthcare and billing platform.",
        "/hospital/pharmacy",
        "Pharmacy module details",
        "Batch and expiry management",
        "Dispensing linked to patient records",
        "Stock valuation and reorder alerts",
        "Integrated billing and medical aid",
        "Audit-ready transaction history");

    yield return Page(
        "pos-system-zimbabwe",
        "POS System Zimbabwe | Point of Sale Software | Pindah",
        "POS system for Zimbabwe retailers and wholesalers. Fast checkout, multi-currency pricing, stock sync, ZIMRA-compliant receipts, and back-office integration.",
        "POS system Zimbabwe, point of sale Zimbabwe, best POS system Zimbabwe, retail ERP Zimbabwe, stock management software Zimbabwe",
        "POS System Zimbabwe",
        "Retail · Point of sale",
        "Pindah POS connects front-of-house sales to inventory and accounting in real time — so every sale updates stock, tax, and your general ledger automatically.",
        "/erp/pointofsale",
        "Explore POS module",
        "USD/ZiG checkout and pricing",
        "Barcode and receipt printing",
        "ZIMRA fiscal integration",
        "Real-time stock deduction",
        "Branch and cashier reporting");

    yield return Page(
        "inventory-management-software-zimbabwe",
        "Inventory Management Software Zimbabwe | Stock & Warehouse System",
        "Inventory and stock management software for Zimbabwe. Multi-location warehouses, FIFO valuation, transfers, and integration with sales and procurement.",
        "inventory management software Zimbabwe, stock management software Zimbabwe, inventory system Zimbabwe, warehouse management software Zimbabwe",
        "Inventory Management Software Zimbabwe",
        "Stock · Warehouses",
        "Track stock across branches and warehouses with inventory software that posts movements to finance automatically — no more reconciling spreadsheets at month-end.",
        "/erp/inventory",
        "Inventory module",
        "Multi-location stock control",
        "Goods issue, transfer, and receipt",
        "FIFO and valuation reporting",
        "Low-stock and reorder alerts",
        "QR and barcode support");

    yield return Page(
        "payroll-software-zimbabwe",
        "Payroll Software Zimbabwe | NSSA & PAYE Compliant | Pindah HR",
        "Payroll software for Zimbabwe. NSSA, PAYE, leave, payslips, and workforce costing integrated with your ERP and general ledger.",
        "payroll software Zimbabwe, HR software Zimbabwe, human resource management Zimbabwe, NSSA PAYE payroll Harare",
        "Payroll Software Zimbabwe",
        "HR & payroll · Zimbabwe statutory",
        "Automate monthly payroll with Zimbabwe statutory deductions, leave balances, and cost allocation — fully integrated with finance and project accounting.",
        "/hr/payroll",
        "Payroll module",
        "NSSA and PAYE calculation",
        "Payslips and employee self-service",
        "Leave and attendance integration",
        "Workforce cost allocation",
        "Audit-ready payroll journals");

    yield return Page(
        "crm-software-zimbabwe",
        "CRM Software Zimbabwe | Sales Pipeline & Customer Management | Pindah",
        "CRM for Zimbabwe businesses. Lead tracking, sales pipeline, quotations, helpdesk, and WhatsApp integration for teams in Harare and nationwide.",
        "CRM Zimbabwe, CRM software Zimbabwe, sales pipeline software Zimbabwe, lead management Zimbabwe, customer management Harare",
        "CRM Software Zimbabwe",
        "Sales · Customer relationships",
        "Pindah CRM unifies leads, opportunities, quotes, and support — with WhatsApp and email in one inbox so your sales team never loses context.",
        "/crm",
        "Explore CRM",
        "Lead capture and scoring",
        "Pipeline and forecast management",
        "Quotations in USD/ZiG",
        "Helpdesk and case management",
        "WhatsApp sales integration");

    yield return Page(
        "fleet-management-software-zimbabwe",
        "Fleet Management Software Zimbabwe | Transport & Logistics | Pindah",
        "Fleet and transport management software for Zimbabwe hauliers. Vehicle tracking, trip planning, driver management, proof of delivery, and cross-border docs.",
        "fleet management software Zimbabwe, transport management software Zimbabwe, logistics ERP Zimbabwe, haulage software Harare",
        "Fleet Management Software Zimbabwe",
        "Logistics · Fleet operations",
        "Orchestrate fleet movements, maintenance, and deliveries with logistics software built for Zimbabwean transport operators and distribution companies.",
        "/logistics",
        "Logistics module",
        "Vehicle and driver registry",
        "Trip scheduling and load planning",
        "GPS tracking integration",
        "Digital proof of delivery",
        "Fuel and cost per km analytics");

    yield return Page(
        "procurement-software-zimbabwe",
        "Procurement Software Zimbabwe | Purchasing & Supplier Management",
        "Procurement software for Zimbabwe. RFQs, purchase orders, budget control, supplier management, and three-way matching integrated with inventory and finance.",
        "procurement software Zimbabwe, supply chain management Zimbabwe, purchasing software Harare, supplier management Zimbabwe",
        "Procurement Software Zimbabwe",
        "Procurement · Supply chain",
        "Control spending with procurement workflows that enforce budgets, approvals, and supplier performance before goods hit your warehouse or ledger.",
        "/scm",
        "SCM module",
        "RFQ and tender comparison",
        "Purchase order approvals",
        "Budget-enforced buying",
        "Supplier scorecards",
        "Three-way invoice matching");

    yield return Page(
        "manufacturing-software-zimbabwe",
        "Manufacturing Software Zimbabwe | Production & MES | Pindah",
        "Manufacturing software for Zimbabwe producers. BOM, production planning, shop floor control, quality, batch traceability, and costing.",
        "manufacturing software Zimbabwe, manufacturing ERP Zimbabwe, production planning software Harare, MES Zimbabwe",
        "Manufacturing Software Zimbabwe",
        "Manufacturing · Shop floor",
        "From raw materials to finished goods, Pindah manufacturing modules give factories real-time visibility over production, quality, and cost.",
        "/manufacturing",
        "Manufacturing module",
        "Bill of materials and recipes",
        "Production scheduling (MRP/MPS)",
        "Shop floor and quality control",
        "Batch traceability and recalls",
        "Actual vs standard costing");

    yield return Page(
        "construction-erp-zimbabwe",
        "Construction ERP Zimbabwe | Project & Cost Control | Pindah",
        "Construction ERP for Zimbabwe contractors. Tendering, contracts, site management, progress billing, subcontractor control, and project costing.",
        "construction ERP Zimbabwe, construction management software Zimbabwe, project cost control Harare, contractor software Zimbabwe",
        "Construction ERP Zimbabwe",
        "Construction · Projects",
        "Manage construction projects from tender to final account with integrated cost control, progress certificates, and site reporting.",
        "/construction",
        "Construction module",
        "Tender and contract management",
        "BoQ and cost control",
        "Progress billing (IPC)",
        "Subcontractor management",
        "Site safety and documentation");

    yield return Page(
        "mining-erp-zimbabwe",
        "Mining ERP Zimbabwe | Resources & Operations Software | Pindah",
        "Mining ERP for Zimbabwe resources sector. Asset management, procurement, fleet, multi-site finance, and compliance reporting for mining operations.",
        "mining ERP Zimbabwe, mining software Zimbabwe, resources sector ERP Harare, mining operations software",
        "Mining ERP Zimbabwe",
        "Mining · Multi-site operations",
        "Pindah supports mining and resources organizations with consolidated finance, asset tracking, procurement, and operational reporting across sites.",
        "/erp",
        "Discuss mining ERP",
        "Multi-entity consolidation",
        "Asset and fleet tracking",
        "Procurement and inventory at scale",
        "Project and cost centre accounting",
        "Compliance-ready audit trails");

    yield return Page(
        "agriculture-erp-zimbabwe",
        "Agriculture ERP Zimbabwe | Farm & Agribusiness Software | Pindah",
        "Agriculture ERP for Zimbabwe agribusiness. Stock, procurement, sales, costing, and financial management for farms, processors, and distributors.",
        "agriculture ERP Zimbabwe, farm management software Zimbabwe, agribusiness software Harare",
        "Agriculture ERP Zimbabwe",
        "Agriculture · Agribusiness",
        "Connect field operations, storage, processing, and sales with an ERP platform tailored to agricultural supply chains in Zimbabwe.",
        "/erp",
        "Agribusiness solutions",
        "Seasonal inventory and batch tracking",
        "Procurement and supplier management",
        "Sales and distribution",
        "Multi-currency finance",
        "Production and yield reporting");

    yield return Page(
        "retail-erp-zimbabwe",
        "Retail ERP Zimbabwe | Shops, Chains & Wholesale | Pindah",
        "Retail ERP for Zimbabwe retailers and wholesalers. POS, inventory, pricing in USD/ZiG, branch reporting, and integrated accounting.",
        "retail ERP Zimbabwe, wholesale ERP Zimbabwe, retail software Zimbabwe, shop management system Harare",
        "Retail ERP Zimbabwe",
        "Retail & wholesale",
        "Run branches, warehouses, and tills on one retail ERP — with live stock, pricing, and financials across every location.",
        "/erp",
        "Retail ERP overview",
        "Multi-branch POS and inventory",
        "Central pricing and promotions",
        "Wholesale credit control",
        "Inter-branch transfers",
        "Consolidated retail reporting");

    yield return Page(
        "ngo-erp-zimbabwe",
        "NGO ERP Zimbabwe | Grant & Programme Management | Pindah",
        "NGO ERP for Zimbabwe non-profits. Fund accounting, donor reporting, procurement controls, payroll, and project visibility.",
        "NGO ERP Zimbabwe, non profit software Zimbabwe, grant management software Harare",
        "NGO ERP Zimbabwe",
        "NGOs · Donor reporting",
        "Meet donor reporting requirements with programme-based accounting, restricted funds, and transparent procurement on one platform.",
        "/erp",
        "NGO solutions",
        "Fund and grant accounting",
        "Donor and project reporting",
        "Procurement compliance",
        "Payroll and volunteer tracking",
        "Audit-ready documentation");

    yield return Page(
        "enterprise-software-harare",
        "Enterprise Software Harare | ERP, CRM & Industry Systems | Pindah",
        "Enterprise software company in Harare. ERP, CRM, healthcare, education, logistics, and custom systems for Zimbabwe organizations.",
        "enterprise software Harare, software company Zimbabwe, best software company Zimbabwe, Pindah software, Pindah Frame, custom software development Zimbabwe",
        "Enterprise Software Harare",
        "Pindah · Harare, Zimbabwe",
        "Pindah Private Limited builds and implements enterprise software for organizations across Harare and Zimbabwe — from finance platforms to hospital and school systems.",
        "/",
        "Explore all solutions",
        "Harare-based software engineering",
        "ERP, CRM, and industry modules",
        "IFRS and ZIMRA aligned finance",
        "Implementation and training",
        "Ongoing support and optimization");

    yield return Page(
        "school-fees-management-software-zimbabwe",
        "School Fees Management Software Zimbabwe | USD & ZiG Billing",
        "School fees software for Zimbabwe. Invoicing, receipts, arrears, sibling discounts, EcoCash reconciliation, and parent statements in USD and ZiG.",
        "school fees management software Zimbabwe, school fee billing Zimbabwe, student billing software Harare, online school fees system",
        "School Fees Management Software Zimbabwe",
        "School fees · Frame SMS",
        "Automate fee billing, reminders, and reconciliation for Zimbabwean schools with multi-currency support and parent-facing statements.",
        "/sms/fees",
        "Fees module",
        "Term and annual fee structures",
        "USD and ZiG invoicing",
        "Arrears and payment plans",
        "EcoCash and bank reconciliation",
        "Parent portal statements");

    yield return Page(
        "student-information-system-zimbabwe",
        "Student Information System Zimbabwe | SIS for Schools | Pindah Frame",
        "Student information system (SIS) for Zimbabwe schools. Central student records, academics, attendance, guardians, and reporting.",
        "student information system Zimbabwe, student management system Zimbabwe, SIS Zimbabwe, school records software Harare",
        "Student Information System Zimbabwe",
        "Student records · Frame",
        "A single student record from admission to alumni — academics, discipline, health notes, guardians, and documents in one secure system.",
        "/sms",
        "Student administration",
        "360° student profiles",
        "Academic history and transcripts",
        "Guardian and emergency contacts",
        "Attendance and behaviour logs",
        "Export and regulatory reporting");

    yield return Page(
        "zimra-compliant-accounting-software",
        "ZIMRA Compliant Accounting Software | Fiscal & Tax Software Zimbabwe",
        "ZIMRA compliant accounting software for Zimbabwe. Fiscal devices, VAT, withholding tax, invoicing, and audit-ready tax reporting.",
        "ZIMRA compliant accounting software, tax compliant accounting software Zimbabwe, fiscalisation software Zimbabwe, VAT software Harare",
        "ZIMRA Compliant Accounting Software",
        "Tax compliance · Zimbabwe",
        "Stay compliant with ZIMRA fiscal requirements while running multi-currency books — invoicing, VAT returns, and withholding tax built into daily workflows.",
        "/accounting",
        "Tax-compliant accounting",
        "Fiscal device integration",
        "VAT and withholding tax",
        "Compliant invoicing templates",
        "Tax audit trails",
        "Statutory reporting support");

    yield return Page(
        "church-management-software-zimbabwe",
        "Church Management Software Zimbabwe | Membership, Giving & Finance | Pindah",
        "Church management software for Zimbabwe. Membership records, offerings and pledges in USD/ZiG, events, groups, and church accounting with Harare-based support.",
        "church management software Zimbabwe, church software Zimbabwe, church membership system Harare, church accounting software Zimbabwe, church ERP Zimbabwe, church giving software",
        "Church Management Software Zimbabwe",
        "Faith organizations · Membership & finance",
        "Pindah helps churches and ministries in Zimbabwe manage members, giving, events, and finances in one platform — with multi-currency support for USD and ZiG offerings and clear reporting for leadership and auditors.",
        "/erp",
        "Discuss church software",
        "Membership and household records",
        "Offerings, pledges, and giving statements",
        "USD and ZiG multi-currency finance",
        "Events, groups, and volunteer tracking",
        "Leadership dashboards and audit trails");

    yield return Page(
        "hotel-management-software-zimbabwe",
        "Hotel Management Software Zimbabwe | PMS, Booking & Front Desk | Pindah",
        "Hotel management software for Zimbabwe. Reservations, front desk, room inventory, guest billing, and accounting for hotels, lodges, and guesthouses in Harare and nationwide.",
        "hotel management software Zimbabwe, hotel PMS Zimbabwe, hotel booking software Harare, lodge management software Zimbabwe, guesthouse software Zimbabwe, hospitality ERP Zimbabwe",
        "Hotel Management Software Zimbabwe",
        "Hospitality · Hotels & lodges",
        "Run reservations, check-in/out, room status, and guest billing on one hotel system built for Zimbabwe's multi-currency payments and local support — from boutique lodges to city hotels.",
        "/erp",
        "Discuss hotel software",
        "Reservations and room inventory",
        "Front desk and guest profiles",
        "Multi-currency guest billing",
        "Housekeeping and room status",
        "Integrated hotel accounting");

    yield return Page(
        "wholesale-erp-zimbabwe",
        "Wholesale ERP Zimbabwe | Distributors & Traders | Pindah",
        "Wholesale ERP for Zimbabwe distributors and traders. Bulk pricing, credit control, warehouse stock, sales orders, and multi-currency accounting for Harare wholesale businesses.",
        "wholesale ERP Zimbabwe, wholesale software Zimbabwe, distributor software Harare, wholesale inventory management Zimbabwe, wholesale accounting software Zimbabwe, trader ERP Zimbabwe",
        "Wholesale ERP Zimbabwe",
        "Wholesale · Distribution",
        "Pindah wholesale ERP connects sales orders, credit limits, warehouse stock, and finance so distributors can quote, pick, deliver, and collect without spreadsheet chaos.",
        "/erp",
        "Explore wholesale ERP",
        "Customer credit limits and aging",
        "Bulk and tiered pricing",
        "Multi-warehouse stock control",
        "Sales orders and delivery notes",
        "USD/ZiG wholesale accounting");

    yield return Page(
        "sap-alternative-zimbabwe",
        "SAP Alternative Zimbabwe | Local ERP Without Enterprise Complexity | Pindah",
        "Looking for an SAP alternative in Zimbabwe? Pindah ERP delivers finance, inventory, HR, and industry modules with local implementation, ZIMRA alignment, and USD/ZiG support — without global ERP overhead.",
        "SAP alternative Zimbabwe, SAP alternative Harare, ERP alternative to SAP Zimbabwe, cheaper than SAP Zimbabwe, local ERP Zimbabwe, Pindah vs SAP",
        "SAP Alternative Zimbabwe",
        "ERP comparison · Zimbabwe buyers",
        "Many Zimbabwean organizations evaluate SAP for integrated finance and operations — then look for a local alternative that matches Zimbabwe's currency, compliance, connectivity, and budget reality. Pindah is built for that market.",
        "/erp-software-zimbabwe",
        "Compare with Pindah ERP",
        "Local Harare implementation and support",
        "Native USD and ZiG multi-currency",
        "ZIMRA and IFRS-oriented finance workflows",
        "Faster phased rollout than global ERP suites",
        "Pricing suited to Zimbabwean organizations");

    yield return Page(
        "pastel-alternative-zimbabwe",
        "Pastel Alternative Zimbabwe | Modern Accounting & ERP | Pindah",
        "Pastel alternative for Zimbabwe businesses ready to move beyond basic accounting. Multi-currency ledgers, inventory, POS, payroll, and ZIMRA-compliant workflows with local support.",
        "Pastel alternative Zimbabwe, Pastel alternative Harare, Sage Pastel alternative Zimbabwe, accounting software instead of Pastel, modern ERP Zimbabwe",
        "Pastel Alternative Zimbabwe",
        "Accounting upgrade · Zimbabwe",
        "If your business has outgrown spreadsheet add-ons or older accounting packages, Pindah offers a modern Zimbabwe-focused alternative with integrated inventory, sales, payroll, and compliance — not just a general ledger.",
        "/accounting-software-zimbabwe",
        "See Pindah Accounting",
        "Multi-currency accounting beyond basic books",
        "Inventory and sales linked to the ledger",
        "ZIMRA fiscal and VAT workflows",
        "Payroll and operations on one platform",
        "Harare-based training and support");

    yield return Page(
        "odoo-alternative-zimbabwe",
        "Odoo Alternative Zimbabwe | Local ERP Implementation | Pindah",
        "Odoo alternative in Zimbabwe with local support, Zimbabwe compliance focus, and industry modules for schools, hospitals, retail, and logistics — implemented by a Harare software company.",
        "Odoo alternative Zimbabwe, Odoo alternative Harare, open source ERP alternative Zimbabwe, local ERP instead of Odoo, Pindah vs Odoo Zimbabwe",
        "Odoo Alternative Zimbabwe",
        "ERP comparison · Local delivery",
        "Open-source and modular ERPs appeal on paper — Zimbabwean teams often need a partner who owns Zimbabwe compliance, multi-currency operations, and on-the-ground implementation. That is where Pindah focuses.",
        "/erp-software-zimbabwe",
        "Explore Pindah ERP",
        "Zimbabwe-first product and support model",
        "Industry modules (schools, healthcare, logistics)",
        "USD/ZiG and ZIMRA-oriented finance",
        "Structured discovery and phased go-live",
        "Single accountable local vendor");

    yield return new SeoLandingPage
    {
        Slug = "heritage-based-curriculum-lessons-zimbabwe",
        Title = "Heritage-Based Curriculum Lessons Zimbabwe | Free HBC Courses | Pindah Course",
        Description = "Free Heritage-Based Curriculum lessons for Zimbabwe primary schools. Grade courses in English, ChiShona, IsiNdebele, Mathematics, Science, Social Science, and PE — Pindah Course by Pindah.org.",
        Keywords = "Heritage-Based Curriculum Zimbabwe, HBC lessons Zimbabwe, PlusOne Heritage-Based Curriculum, primary school lessons Zimbabwe, free curriculum lessons Zimbabwe, Pindah Course, heritage based curriculum primary school",
        H1 = "Heritage-Based Curriculum Lessons Zimbabwe",
        Kicker = "Pindah Course · Education products",
        Lead = "Pindah Course is a Pindah.org product: child-ready lessons for Zimbabwe's Heritage-Based Curriculum, organised by subject and grade, with the PlusOne teacher-guide PDFs beside each course. It sits with Frame school management and the Basa client portal in the Pindah education offering.",
        ModuleUrl = "https://courses.edtech.co.zw",
        ModuleLabel = "Open Pindah Course",
        Benefits =
        [
            "Lessons by subject and grade for ECD through Grade 7",
            "English, ChiShona, IsiNdebele, Mathematics, Science, Social Science, and PE",
            "Stories, vocabulary tables, practice, and charts on every lesson",
            "PlusOne Heritage-Based Curriculum PDFs linked from each course",
            "Part of the Pindah.org family with Frame SMS and Basa"
        ],
        Faqs =
        [
            new()
            {
                Question = "What is Pindah Course?",
                Answer = "Pindah Course is a free Heritage-Based Curriculum lesson site from Pindah Private Limited. Open it at courses.edtech.co.zw. It is part of the Pindah.org product family, alongside Frame school management (frame.pindah.org) and the Basa client portal (basa.pindah.org)."
            },
            new()
            {
                Question = "Is Pindah Course the same as Frame?",
                Answer = "No. Frame at frame.pindah.org is school administration — enrollment, fees, attendance, and ZIMSEC grading. Pindah Course is the teaching content: grade courses and lessons. Schools often use both."
            },
            new()
            {
                Question = "How do organisations log in to Pindah products?",
                Answer = "Client organisations use Basa at basa.pindah.org. Product information and demos live on pindah.org. Pindah Course itself is a public lesson site at courses.edtech.co.zw."
            },
            new()
            {
                Question = "Are the lessons aligned to Zimbabwe's Heritage-Based Curriculum?",
                Answer = "Yes. Courses follow PlusOne Heritage-Based Curriculum teacher guides by subject and grade, with child-ready steps, practice, and the original PDFs beside each course."
            }
        ]
    };

    yield return new SeoLandingPage
    {
        Slug = "pindah-course",
        Title = "Pindah Course | Free Primary Lessons Zimbabwe | Pindah.org",
        Description = "Pindah Course is Pindah.org's free primary lesson platform for Zimbabwe — Heritage-Based Curriculum courses, grade by grade, with Frame and Basa in the same product family.",
        Keywords = "Pindah Course, Pindah.org courses, Pindah education products, free primary lessons Zimbabwe, Pindah Frame, Basa Pindah, courses.edtech.co.zw",
        H1 = "Pindah Course",
        Kicker = "Pindah.org products · Education",
        Lead = "Pindah Course is how Pindah publishes classroom-ready Heritage-Based Curriculum lessons. It is a first-class Pindah.org product, next to Frame (schools) and Basa (client login).",
        ModuleUrl = "https://courses.edtech.co.zw",
        ModuleLabel = "Go to Pindah Course",
        Benefits =
        [
            "Public lesson site at courses.edtech.co.zw",
            "Works beside Frame school management at frame.pindah.org",
            "Organisations sign in through Basa at basa.pindah.org",
            "Company home and demos at pindah.org",
            "Zimbabwe Heritage-Based Curriculum, grade by grade"
        ],
        Faqs =
        [
            new()
            {
                Question = "Where do I open Pindah Course?",
                Answer = "Use https://courses.edtech.co.zw. It is listed with Pindah's other products on pindah.org."
            },
            new()
            {
                Question = "Which Pindah products sit with Pindah Course?",
                Answer = "Frame school management (https://frame.pindah.org), the Basa client portal (https://basa.pindah.org), and the main Pindah site (https://pindah.org). Education buyers typically look at Frame plus Pindah Course together."
            },
            new()
            {
                Question = "Is Pindah Course free?",
                Answer = "The lesson site is free to use. Frame school administration and other Pindah enterprise modules are licensed separately — start at pindah.org or basa.pindah.org."
            },
            new()
            {
                Question = "Do you support schools in Harare and nationwide?",
                Answer = "Yes. Pindah is a Harare software company. Frame, Basa, and Pindah Course are offered to schools across Zimbabwe."
            }
        ]
    };

    yield return new SeoLandingPage
    {
        Slug = "primary-school-lessons-zimbabwe",
        Title = "Primary School Lessons Zimbabwe | HBC Grade Courses | Pindah",
        Description = "Primary school lessons for Zimbabwe: Heritage-Based Curriculum grade courses with stories, practice, and teacher-guide PDFs. A Pindah.org product at courses.edtech.co.zw.",
        Keywords = "primary school lessons Zimbabwe, grade 1 to 7 lessons Zimbabwe, ECD lessons Zimbabwe, free primary school resources Zimbabwe, Heritage-Based Curriculum primary, Pindah Course",
        H1 = "Primary School Lessons Zimbabwe",
        Kicker = "Pindah Course · Heritage-Based Curriculum",
        Lead = "Find grade courses for Zimbabwe primary classrooms — ECD to Grade 7 — on Pindah Course. Pair the lessons with Frame if you also need school fees, attendance, and parent portals.",
        ModuleUrl = "https://courses.edtech.co.zw",
        ModuleLabel = "Browse grade courses",
        Benefits =
        [
            "Grade courses instead of a pile of unsorted files",
            "ChiShona, IsiNdebele, English, Maths, Science, and more",
            "Child-ready steps a helper can follow at home",
            "Teacher-guide PDFs on the matching course",
            "School admin stays on Frame; lessons stay on Pindah Course"
        ],
        Faqs =
        [
            new()
            {
                Question = "Can parents use Pindah Course at home?",
                Answer = "Yes. Lessons are written for a child and a helper. Schools that run Frame can still point families to courses.edtech.co.zw for the Heritage-Based Curriculum content."
            },
            new()
            {
                Question = "How does this relate to Pindah Frame?",
                Answer = "Frame (frame.pindah.org) runs the school. Pindah Course teaches the curriculum. Both are Pindah.org products; Basa (basa.pindah.org) is the client login for licensed Pindah systems."
            },
            new()
            {
                Question = "Which grades are covered?",
                Answer = "Heritage-Based Curriculum coverage follows the PlusOne teacher guides on the site — typically ECD through Grade 7 by subject. Open courses.edtech.co.zw and pick a subject."
            },
            new()
            {
                Question = "Where do I read about Pindah as a company?",
                Answer = "https://pindah.org — enterprise software for Zimbabwe, including education, ERP, accounting, and healthcare."
            }
        ]
    };
  }
}
