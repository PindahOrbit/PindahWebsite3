using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Areas.Identity.Data;
using PindahWebsite3.Models;

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

        builder.Entity<News>(entity =>
        {
            entity.HasIndex(n => n.Slug).IsUnique();
            entity.HasIndex(n => new { n.IsFeatured, n.FeaturedRank });
            entity.HasIndex(n => n.Status);
            entity.HasOne(n => n.Author)
                .WithMany()
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }

    public DbSet<ZimsecCategory> ZimsecCategories { get; set; }
    public DbSet<ZimsecDocument> ZimsecDocuments { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Download> Downloads { get; set; }
}
