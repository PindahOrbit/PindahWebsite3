namespace PindahWebsite3.Models;

public class ProductGuideInfo
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public string? Summary { get; init; }
    public DateTime LastModifiedUtc { get; init; }
    public bool HasPdf { get; init; }
    public string? PdfUrl { get; init; }
}

public class ProductGuideViewModel
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Html { get; init; }
    public DateTime LastModifiedUtc { get; init; }
    public bool HasPdf { get; init; }
    public string? PdfUrl { get; init; }
}
