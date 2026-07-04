using Microsoft.EntityFrameworkCore;
using PindahWebsite3.Models.ZimsecLibrary;

namespace PindahWebsite3.Data;

public class ZimsecContext : DbContext
{
    public ZimsecContext(DbContextOptions<ZimsecContext> options) : base(options) { }

    public DbSet<ZimsecStudent> Students => Set<ZimsecStudent>();
    public DbSet<ZimsecLibraryDocument> Documents => Set<ZimsecLibraryDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ZimsecStudent>(e =>
        {
            e.HasIndex(x => x.PhoneNumber).IsUnique();
        });

        modelBuilder.Entity<ZimsecLibraryDocument>(e =>
        {
            e.HasIndex(x => x.RelativePath).IsUnique();
            e.HasIndex(x => x.Level);
            e.HasIndex(x => x.SubjectSlug);
            e.HasIndex(x => new { x.Level, x.SubjectSlug });
        });
    }
}
