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
