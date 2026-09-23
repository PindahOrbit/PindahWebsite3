namespace PindahWebsite3.ViewModels;

public sealed class ProductGuideListItem
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public DateTime UpdatedUtc { get; init; }
    public bool HasPdf { get; init; }
}

public sealed class ProductGuideDetailViewModel
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Html { get; init; }
    public DateTime UpdatedUtc { get; init; }
    public bool HasPdf { get; init; }
}
