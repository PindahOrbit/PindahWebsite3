using System.ComponentModel.DataAnnotations;

namespace PindahWebsite3.Models;

public class NewsSaveModel
{
    public int? Id { get; set; }

    [Required]
    [MaxLength(300)]
    public string Heading { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string CoverImageUrl { get; set; } = string.Empty;

    [MaxLength(400)]
    public string? Slug { get; set; }

    public NewsStatus Status { get; set; } = NewsStatus.Published;

    public bool IsFeatured { get; set; }
}
