using PindahWebsite3.Services.Zimsec;

namespace PindahWebsite3.ViewModels;

public class ZimsecIndexViewModel
{
    public string? LoginError { get; set; }
    public string? RegisterError { get; set; }
    public string? RegisterSuccess { get; set; }
    public int TotalDocuments { get; set; }
    public int TotalSubjects { get; set; }
    public IReadOnlyList<ZimsecLevelNode> PreviewTree { get; set; } = Array.Empty<ZimsecLevelNode>();
}

public class ZimsecLibraryViewModel
{
    public string PhoneNumber { get; set; } = string.Empty;
    public IReadOnlyList<ZimsecLevelNode> Tree { get; set; } = Array.Empty<ZimsecLevelNode>();
    public string? SelectedLevel { get; set; }
    public string? SelectedSubject { get; set; }
    public string? SelectedLevelDisplay { get; set; }
    public string? SelectedSubjectDisplay { get; set; }
    public string SearchQuery { get; set; } = string.Empty;
    public ZimsecSearchResult? SearchResult { get; set; }
    public IReadOnlyList<ZimsecDocumentListItem> BrowseDocuments { get; set; } = Array.Empty<ZimsecDocumentListItem>();
}

public class ZimsecDocumentViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string LevelDisplay { get; set; } = string.Empty;
    public string SubjectDisplay { get; set; } = string.Empty;
    public string SubjectSlug { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public int PageCount { get; set; }
    public string ReturnUrl { get; set; } = "/Zimsec/Library";
}
