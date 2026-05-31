using System.ComponentModel.DataAnnotations;

namespace PindahWebsite3.Models;

public class NewsSaveModel
{
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
}
