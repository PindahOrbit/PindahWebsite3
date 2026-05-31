using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Areas.Identity.Data;
using PindahWebsite3.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PindahWebsite3.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(PindahWebsite3Context context, UserManager<PindahWebsite3User> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            // Creates the database and tables if they don't exist
            context.Database.EnsureCreated();

            // Seed Roles
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
                // Ensure the schema handles our new property for existing Db
                await context.Database.ExecuteSqlRawAsync("ALTER TABLE ZimsecDocuments ADD COLUMN UploadedByUserId TEXT NULL;");
            }
            catch
            {
                // Ignored - probably column already exists
            }

            var adminEmail = "admin@zimsec.pindah.org";
            var adminPassword = "SuperStrongPassword123!@#_2026_SecureAdmin!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new PindahWebsite3User 
                { 
                    UserName = adminEmail, 
                    Email = adminEmail, 
                    EmailConfirmed = true 
                };
                
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    // Store credentials securely inside wwwroot/credentials.json
                    string credPath = Path.Combine(env.WebRootPath, "credentials.json");
                    string credContent = $"{{\n  \"username\": \"{adminEmail}\",\n  \"password\": \"{adminPassword}\"\n}}";
                    await File.WriteAllTextAsync(credPath, credContent);
                }
            }

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
    }
}
