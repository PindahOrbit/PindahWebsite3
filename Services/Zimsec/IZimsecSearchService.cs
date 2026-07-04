namespace PindahWebsite3.Services.Zimsec;

public record ZimsecSearchHit(
    int DocumentId,
    string Title,
    string FileName,
    string Level,
    string LevelDisplay,
    string SubjectSlug,
    string SubjectDisplay,
    double Score,
    string Snippet,
    IReadOnlyList<string> HighlightTerms);

public record ZimsecSearchResult(
    string Query,
    int TotalCount,
    IReadOnlyList<ZimsecSearchHit> Hits,
    IReadOnlyDictionary<string, int> LevelFacets,
    IReadOnlyDictionary<string, int> SubjectFacets);

public interface IZimsecSearchService
{
    Task<ZimsecSearchResult> SearchAsync(
        string? query,
        string? level = null,
        string? subject = null,
        int limit = 40,
        CancellationToken cancellationToken = default);
}
