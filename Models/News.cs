using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PindahWebsite3.Areas.Identity.Data;

namespace PindahWebsite3.Models;

public class News
{
    public int Id { get; set; }

    [Required]
    [MaxLength(300)]
    public string Heading { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    [MaxLength(400)]
    public string Slug { get; set; } = string.Empty;

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime? DatePublished { get; set; }

    public DateTime? DateModified { get; set; }

    [MaxLength(500)]
    public string CoverImageUrl { get; set; } = string.Empty;

    public NewsStatus Status { get; set; } = NewsStatus.Draft;

    /// <summary>Pinned to the home page Insights section (max <see cref="CmsConstants.MaxFeaturedSlots"/>).</summary>
    public bool IsFeatured { get; set; }

    /// <summary>Display order among featured articles (1 = first). Null when not featured.</summary>
    public int? FeaturedRank { get; set; }

    [MaxLength(450)]
    public string? AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public PindahWebsite3User? Author { get; set; }

    [NotMapped]
    public string AuthorDisplayName =>
        Author?.Email ?? Author?.UserName ?? "Pindah";
}
