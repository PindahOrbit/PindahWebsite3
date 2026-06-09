using System.ComponentModel.DataAnnotations;

namespace PindahWebsite3.Models;

public class DownloadSaveModel
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    [Url]
    public string FileUrl { get; set; } = string.Empty;

    [MaxLength(50)]
    public string FileType { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Platform { get; set; } = string.Empty;

    public bool IsPublished { get; set; } = true;

    public int SortOrder { get; set; }
}
