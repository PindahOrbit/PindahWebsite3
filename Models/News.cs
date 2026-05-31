using System.ComponentModel.DataAnnotations;

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

    [MaxLength(500)]
    public string CoverImageUrl { get; set; } = string.Empty;
}
