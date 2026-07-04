using System.ComponentModel.DataAnnotations;

namespace PindahWebsite3.Models.ZimsecLibrary;

public class ZimsecLibraryDocument
{
    public int Id { get; set; }

    /// <summary>Path relative to wwwroot/zimsec, e.g. o-level/mathematics/paper.pdf</summary>
    [Required, MaxLength(512)]
    public string RelativePath { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string FileName { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(16)]
    public string Level { get; set; } = string.Empty;

    [Required, MaxLength(128)]
    public string SubjectSlug { get; set; } = string.Empty;

    [Required, MaxLength(128)]
    public string SubjectDisplay { get; set; } = string.Empty;

    public long FileSizeBytes { get; set; }

    public int PageCount { get; set; }

    public DateTime IndexedAtUtc { get; set; } = DateTime.UtcNow;

    [MaxLength(64)]
    public string? ContentHash { get; set; }

    /// <summary>Full extracted PDF text for display snippets; FTS mirror lives in DocumentSearch.</summary>
    public string ExtractedText { get; set; } = string.Empty;
}
