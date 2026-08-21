using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PindahWebsite3.Areas.Identity.Data;
using PindahWebsite3.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
 
namespace PindahWebsite3.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            PindahWebsite3Context context,
            UserManager<PindahWebsite3User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            await EnsureDatabaseSchemaAsync(context);
            await SeedAdminUserAsync(context, userManager, roleManager, configuration);
            await SeedDownloadsAsync(context);
            await SeedNewsAsync(context);

            if (!context.ZimsecCategories.Any())
            {
                var oLevel = new ZimsecCategory { Name = "Zimsec O Level" };
                var aLevel = new ZimsecCategory { Name = "Zimsec A Level" };
                var grade7 = new ZimsecCategory { Name = "Zimsec Grade 7" };
                
                context.ZimsecCategories.AddRange(oLevel, aLevel, grade7);
                context.SaveChanges();

                context.ZimsecCategories.AddRange(
                    new ZimsecCategory { Name = "Mathematics (4004)", ParentCategoryId = oLevel.Id },
                    new ZimsecCategory { Name = "English Language (1122)", ParentCategoryId = oLevel.Id },
                    new ZimsecCategory { Name = "Combined Science (4003)", ParentCategoryId = oLevel.Id },
                    new ZimsecCategory { Name = "History (2167)", ParentCategoryId = oLevel.Id },
                    new ZimsecCategory { Name = "Geography (2248)", ParentCategoryId = oLevel.Id }
                );
                
                context.ZimsecCategories.AddRange(
                    new ZimsecCategory { Name = "Mathematics (6042)", ParentCategoryId = aLevel.Id },
                    new ZimsecCategory { Name = "Physics (6052)", ParentCategoryId = aLevel.Id },
                    new ZimsecCategory { Name = "Chemistry (6064)", ParentCategoryId = aLevel.Id }
                );

                context.SaveChanges();
            }
        }

        public static async Task SeedAdminUserAsync(
            PindahWebsite3Context context,
            UserManager<PindahWebsite3User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            await EnsureDatabaseSchemaAsync(context);

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Contributor"))
            {
                await roleManager.CreateAsync(new IdentityRole("Contributor"));
            }

            try
            {
                await context.Database.ExecuteSqlRawAsync("ALTER TABLE ZimsecDocuments ADD COLUMN UploadedByUserId TEXT NULL;");
            }
            catch
            {
                // Column likely already exists.
            }

            var adminEmail = configuration["Admin:Email"];
            var adminPassword = configuration["Admin:Password"];

            if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new PindahWebsite3User
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
                else
                {
                    var userChanged = false;

                    if (adminUser.UserName != adminEmail)
                    {
                        adminUser.UserName = adminEmail;
                        userChanged = true;
                    }

                    if (adminUser.Email != adminEmail)
                    {
                        adminUser.Email = adminEmail;
                        userChanged = true;
                    }

                    if (!adminUser.EmailConfirmed)
                    {
                        adminUser.EmailConfirmed = true;
                        userChanged = true;
                    }

                    if (userChanged)
                    {
                        await userManager.UpdateAsync(adminUser);
                    }

                    if (!await userManager.CheckPasswordAsync(adminUser, adminPassword))
                    {
                        if (await userManager.HasPasswordAsync(adminUser))
                        {
                            await userManager.RemovePasswordAsync(adminUser);
                        }

                        await userManager.AddPasswordAsync(adminUser, adminPassword);
                    }

                    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }

            var usersMissingNormalizedFields = await context.Users
                .Where(u => u.NormalizedUserName == null || u.NormalizedEmail == null)
                .ToListAsync();

            foreach (var user in usersMissingNormalizedFields)
            {
                if (!string.IsNullOrWhiteSpace(user.UserName))
                {
                    user.NormalizedUserName = user.UserName.ToUpperInvariant();
                }

                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    user.NormalizedEmail = user.Email.ToUpperInvariant();
                }
            }

            if (usersMissingNormalizedFields.Count > 0)
            {
                await context.SaveChangesAsync();
            }
        }

        private static async Task EnsureDatabaseSchemaAsync(PindahWebsite3Context context)
        {
            var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
            var hasMigrationHistory = appliedMigrations.Any();
            var hasLegacyTables = await TableExistsAsync(context, "ZimsecCategories");

            if (hasMigrationHistory || !hasLegacyTables)
            {
                await context.Database.MigrateAsync();
            }

            if (!await TableExistsAsync(context, "News"))
            {
                await context.Database.ExecuteSqlRawAsync("""
                    CREATE TABLE IF NOT EXISTS "News" (
                        "Id" INTEGER NOT NULL CONSTRAINT "PK_News" PRIMARY KEY AUTOINCREMENT,
                        "Heading" TEXT NOT NULL,
                        "Content" TEXT NOT NULL,
                        "Slug" TEXT NOT NULL,
                        "DateCreated" TEXT NOT NULL,
                        "CoverImageUrl" TEXT NOT NULL
                    );
                    """);
            }

            if (!await TableExistsAsync(context, "Downloads"))
            {
                await context.Database.ExecuteSqlRawAsync("""
                    CREATE TABLE IF NOT EXISTS "Downloads" (
                        "Id" INTEGER NOT NULL CONSTRAINT "PK_Downloads" PRIMARY KEY AUTOINCREMENT,
                        "Title" TEXT NOT NULL,
                        "Description" TEXT NOT NULL,
                        "FileUrl" TEXT NOT NULL,
                        "FileType" TEXT NOT NULL,
                        "Platform" TEXT NOT NULL,
                        "IsPublished" INTEGER NOT NULL,
                        "SortOrder" INTEGER NOT NULL,
                        "DateAdded" TEXT NOT NULL
                    );
                    """);
            }
        }

        private static async Task SeedDownloadsAsync(PindahWebsite3Context context)
        {
            const string seedUrl = "https://storage.pindah.org/mobile-apps/app-release.apk";

            if (await context.Downloads.AnyAsync(d => d.FileUrl == seedUrl))
            {
                return;
            }

            context.Downloads.Add(new Download
            {
                Title = "Pindah Mobile App (Android)",
                Description = "Android release build of the Pindah mobile application.",
                FileUrl = seedUrl,
                FileType = "APK",
                Platform = "Android",
                IsPublished = true,
                SortOrder = 0,
                DateAdded = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
        }

        private static async Task SeedNewsAsync(PindahWebsite3Context context)
        {
            var articles = GetSeoSeedArticles();
            var seedSlugs = articles.Select(a => a.Slug).ToList();
            var existingSlugs = await context.News
                .Where(n => seedSlugs.Contains(n.Slug))
                .Select(n => n.Slug)
                .ToListAsync();

            var missing = articles.Where(a => !existingSlugs.Contains(a.Slug)).ToList();
            if (missing.Count == 0)
            {
                return;
            }

            context.News.AddRange(missing);
            await context.SaveChangesAsync();
        }

        private static List<News> GetSeoSeedArticles()
        {
            var cover = "https://storage.pindah.org/IMAGES/pindah-blog-default.jpg";
            var now = DateTime.UtcNow;

            return
            [
                new News
                {
                    Heading = "How Zimbabwean Schools Can Digitize Fee Collection in USD and ZiG",
                    Slug = "zimbabwe-school-fee-collection-usd-zig",
                    CoverImageUrl = cover,
                    DateCreated = now.AddDays(-12),
                    Content = """
                        <p>School fees in Zimbabwe rarely stay in one currency. Parents pay in USD, ZiG, EcoCash, or bank transfer — often in the same term. Finance officers then reconcile statements by hand, chase arrears, and rebuild reports for boards and auditors.</p>
                        <h2>Why fee spreadsheets break at scale</h2>
                        <p>Once enrollment grows past a few hundred learners, spreadsheet fee books create three problems: duplicate invoices, unclear arrears by student, and no reliable sibling or bursary adjustments. Parent WhatsApp queries multiply because nobody has a single source of truth.</p>
                        <h2>What a Zimbabwe-ready school fees module needs</h2>
                        <ul>
                          <li>Term and annual fee structures with USD and ZiG line items</li>
                          <li>Receipts that map payments to student accounts and payment methods</li>
                          <li>Arrears aging and payment plans leadership can act on</li>
                          <li>Parent-facing statements without exporting PDFs from Excel every week</li>
                        </ul>
                        <p>Pindah Frame is built for these realities. See our <a href="/school-fees-management-software-zimbabwe">school fees management software Zimbabwe</a> page and the broader <a href="/school-management-software-zimbabwe">school management software</a> overview.</p>
                        <h2>Implementation tip</h2>
                        <p>Start with the fee year already in progress: migrate opening balances per learner, lock the fee structures, then train cashiers and bursars on receipting before opening the parent portal. Digitization fails when you roll out portals before the ledger is clean.</p>
                        """
                },
                new News
                {
                    Heading = "ZIMRA Fiscal Compliance Checklist for Zimbabwe Accounting Software Buyers",
                    Slug = "zimra-fiscal-compliance-checklist-accounting-software",
                    CoverImageUrl = cover,
                    DateCreated = now.AddDays(-10),
                    Content = """
                        <p>Buying accounting software in Zimbabwe is not only about invoices and bank reconciliation. Finance teams must ask how the platform supports ZIMRA fiscalisation, VAT, withholding tax, and audit trails — or they will re-buy in two years.</p>
                        <h2>Questions to ask every vendor</h2>
                        <ol>
                          <li>How do fiscal device receipts relate to the sales invoice in the ledger?</li>
                          <li>Can VAT returns be produced from posted transactions without a side spreadsheet?</li>
                          <li>How is withholding tax captured on supplier payments?</li>
                          <li>Are audit logs immutable for who changed invoices, rates, and journals?</li>
                          <li>Does multi-currency posting (USD/ZiG) preserve tax reporting clarity?</li>
                        </ol>
                        <h2>What “ZIMRA compliant” should mean in practice</h2>
                        <p>Compliance is a workflow, not a slogan. Day-to-day cashiers and accountants should produce fiscalised documents as part of normal sales — not as a weekend catch-up exercise before a return is due.</p>
                        <p>Explore <a href="/zimra-compliant-accounting-software">ZIMRA compliant accounting software</a> and <a href="/accounting-software-zimbabwe">accounting software Zimbabwe</a> for how Pindah approaches tax-aligned finance.</p>
                        """
                },
                new News
                {
                    Heading = "ERP vs Spreadsheets: When Harare Growing Businesses Should Make the Switch",
                    Slug = "erp-vs-spreadsheets-harare-growing-businesses",
                    CoverImageUrl = cover,
                    DateCreated = now.AddDays(-8),
                    Content = """
                        <p>Most Harare SMEs start on Excel — and many stay too long. The tipping point is rarely “we want ERP.” It is stock that never matches sales, month-end closes that take weeks, and managers who cannot trust yesterday’s numbers.</p>
                        <h2>Signals you have outgrown spreadsheets</h2>
                        <ul>
                          <li>Multiple versions of “the” stock file circulate on WhatsApp</li>
                          <li>Sales people quote prices that finance cannot reconcile</li>
                          <li>Payroll, inventory, and bank books never agree without manual journals</li>
                          <li>New branches or warehouses make consolidation painful</li>
                        </ul>
                        <h2>What to implement first</h2>
                        <p>Do not boil the ocean. A practical Zimbabwe rollout often starts with inventory + invoicing + general ledger, then POS or payroll, then CRM. Local support and multi-currency matter as much as feature lists.</p>
                        <p>Read more on <a href="/erp-software-zimbabwe">ERP software Zimbabwe</a> and <a href="/enterprise-software-harare">enterprise software Harare</a>, or compare options if you are evaluating a <a href="/sap-alternative-zimbabwe">SAP alternative in Zimbabwe</a>.</p>
                        """
                },
                new News
                {
                    Heading = "Case Study Pattern: Digitize a Private Clinic in Zimbabwe Without Disrupting OPD",
                    Slug = "case-study-private-clinic-digitization-zimbabwe",
                    CoverImageUrl = cover,
                    DateCreated = now.AddDays(-6),
                    Content = """
                        <p>Private clinics in Zimbabwe want electronic records and medical aid billing — without slowing the queue at 08:00. A phased clinic digitization pattern works better than a big-bang “go live Monday” approach.</p>
                        <h2>Phase 1 — Front desk and patient identity</h2>
                        <p>Register patients once, verify medical aid eligibility where possible, and stop creating duplicate folders. Capture demographics cleanly before clinical notes go digital.</p>
                        <h2>Phase 2 — OPD consultation and prescribing</h2>
                        <p>Doctors move onto SOAP-style notes and e-prescribing after the MPI is stable. Train one consulting room first, then expand.</p>
                        <h2>Phase 3 — Pharmacy, billing, and claims</h2>
                        <p>Link dispensing to stock and billing. Medical aid claims succeed when clinical and financial data already live in one system.</p>
                        <p>See <a href="/clinic-software-zimbabwe">clinic software Zimbabwe</a> and <a href="/hospital-management-system-zimbabwe">hospital management system Zimbabwe</a> for the modules that support this journey.</p>
                        <h2>Outcome pattern</h2>
                        <p>Clinics that sequence rollout this way typically reduce lost files, accelerate claims follow-up, and give owners a same-day view of cash and medical aid receivables.</p>
                        """
                },
                new News
                {
                    Heading = "Wholesale Distributors in Zimbabwe: Credit Control, Stock, and Multi-Currency Reality",
                    Slug = "wholesale-distributors-zimbabwe-credit-stock-multicurrency",
                    CoverImageUrl = cover,
                    DateCreated = now.AddDays(-4),
                    Content = """
                        <p>Wholesale trading in Zimbabwe rewards speed — and punishes weak credit control. Distributors who sell on account without live aging and stock reservation discover stockouts and bad debt at the same time.</p>
                        <h2>Core wholesale workflows that must be integrated</h2>
                        <ul>
                          <li>Customer credit limits enforced at order entry</li>
                          <li>Tiered pricing for retailers vs walk-in cash buyers</li>
                          <li>Warehouse picks linked to delivery notes</li>
                          <li>USD/ZiG receipts posted to the correct customer account</li>
                        </ul>
                        <p>That is the job of a <a href="/wholesale-erp-zimbabwe">wholesale ERP for Zimbabwe</a>, not a POS alone. Retail tills and wholesale credit books behave differently; treat them as related but separate workflows.</p>
                        <h2>Where Pindah fits</h2>
                        <p>Pindah connects sales orders, inventory, and accounting so finance sees the same stock and debtor picture operations sees on the warehouse floor. Explore <a href="/inventory-management-software-zimbabwe">inventory management software Zimbabwe</a> alongside wholesale ERP.</p>
                        """
                },
                new News
                {
                    Heading = "Choosing a Local ERP Alternative in Zimbabwe: Cost, Compliance, and Support",
                    Slug = "choosing-local-erp-alternative-zimbabwe",
                    CoverImageUrl = cover,
                    DateCreated = now.AddDays(-2),
                    Content = """
                        <p>Global ERP brands and open-source platforms appear in many Zimbabwe RFPs. The decisive questions are usually local: Who implements? Who supports after go-live? How does the system handle ZiG, ZIMRA, and intermittent connectivity?</p>
                        <h2>Compare on Zimbabwe operating conditions</h2>
                        <table>
                          <thead><tr><th>Factor</th><th>What to demand</th></tr></thead>
                          <tbody>
                            <tr><td>Currency</td><td>Native USD and ZiG posting and reporting</td></tr>
                            <tr><td>Tax</td><td>Fiscal and VAT workflows that match daily operations</td></tr>
                            <tr><td>Support</td><td>Harare-accessible implementation and training</td></tr>
                            <tr><td>Scope</td><td>Phased modules — not a 24-month mega-project</td></tr>
                            <tr><td>Total cost</td><td>Licences + local consulting + change management</td></tr>
                          </tbody>
                        </table>
                        <p>If you are benchmarking against global suites, read our pages on a <a href="/sap-alternative-zimbabwe">SAP alternative Zimbabwe</a>, <a href="/pastel-alternative-zimbabwe">Pastel alternative Zimbabwe</a>, and <a href="/odoo-alternative-zimbabwe">Odoo alternative Zimbabwe</a> — then evaluate Pindah on a discovery workshop, not a feature checkbox alone.</p>
                        """
                },
                new News
                {
                    Heading = "Hotel and Lodge Software Needs in Zimbabwe: Beyond a Booking Spreadsheet",
                    Slug = "hotel-lodge-software-needs-zimbabwe",
                    CoverImageUrl = cover,
                    DateCreated = now.AddDays(-1),
                    Content = """
                        <p>Lodges and city hotels in Zimbabwe often run occupancy in Excel, payments in EcoCash statements, and housekeeping on WhatsApp. That works until double bookings, unclear deposits, and month-end room revenue become daily firefighting.</p>
                        <h2>Minimum hospitality stack</h2>
                        <ul>
                          <li>Reservations with room types and rate plans</li>
                          <li>Front desk check-in/out and guest folio billing</li>
                          <li>Multi-currency deposits and final settlement</li>
                          <li>Housekeeping room status visible to reception</li>
                          <li>Posting of room revenue into accounting</li>
                        </ul>
                        <p>See <a href="/hotel-management-software-zimbabwe">hotel management software Zimbabwe</a> for how Pindah approaches hospitality operations, and connect finance through <a href="/accounting-software-zimbabwe">accounting software Zimbabwe</a>.</p>
                        """
                },
                new News
                {
                    Heading = "Church Administration Software in Zimbabwe: Membership, Giving, and Accountability",
                    Slug = "church-administration-software-zimbabwe-membership-giving",
                    CoverImageUrl = cover,
                    DateCreated = now,
                    Content = """
                        <p>Churches and ministries need more than a WhatsApp announcement group. Leadership requires accurate membership rolls, offering records in mixed currencies, and financial reports that satisfy internal accountability and external auditors.</p>
                        <h2>Typical church software requirements</h2>
                        <ul>
                          <li>Household and member directories with pastoral notes access control</li>
                          <li>Offering and pledge tracking in USD and ZiG</li>
                          <li>Event and group attendance</li>
                          <li>Finance reports for elders, boards, and AGMs</li>
                        </ul>
                        <p>Explore <a href="/church-management-software-zimbabwe">church management software Zimbabwe</a> and related finance capabilities under <a href="/accounting-software-zimbabwe">accounting software Zimbabwe</a>.</p>
                        <p>Digitization succeeds when pastors and treasurers agree on workflows before data migration — start with membership cleansing and offering categories, then automate statements.</p>
                        """
                }
            ];
        }

        private static async Task<bool> TableExistsAsync(PindahWebsite3Context context, string tableName)
        {
            var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = $name;";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "$name";
            parameter.Value = tableName;
            command.Parameters.Add(parameter);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
    }
}
