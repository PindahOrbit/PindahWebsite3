using System.ComponentModel.DataAnnotations;

namespace PindahWebsite3.Models;

public class VideoGuideSaveModel
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    [Display(Name = "YouTube URL")]
    public string YouTubeUrl { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public bool IsPublished { get; set; } = true;

    public int SortOrder { get; set; }
}
