using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Areas.Identity.Data;

namespace PindahWebsite3.Data;

public class PindahWebsite3Context : IdentityDbContext<PindahWebsite3User>
{
    public PindahWebsite3Context(DbContextOptions<PindahWebsite3Context> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
    
    public DbSet<PindahWebsite3.Models.ZimsecCategory> ZimsecCategories { get; set; }
    public DbSet<PindahWebsite3.Models.ZimsecDocument> ZimsecDocuments { get; set; }
    public DbSet<PindahWebsite3.Models.News> News { get; set; }
}
