namespace PindahWebsite3.Services.Zimsec;

public record ZimsecSubjectNode(string Slug, string DisplayName, int DocumentCount);

public record ZimsecLevelNode(string Slug, string DisplayName, IReadOnlyList<ZimsecSubjectNode> Subjects);

public record ZimsecDocumentListItem(
    int Id,
    string Title,
    string FileName,
    string Level,
    string SubjectSlug,
    string SubjectDisplay,
    long FileSizeBytes,
    int PageCount);

public interface IZimsecCatalogService
{
    string LibraryRoot { get; }
    IReadOnlyList<ZimsecLevelNode> GetTreeFromDisk();
    string FormatSubjectDisplay(string slug);
    string FormatLevelDisplay(string slug);
    string? ResolvePhysicalPath(string relativePath);
}
