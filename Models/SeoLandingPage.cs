namespace PindahWebsite3.Models;

public sealed class SeoLandingPage
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Keywords { get; init; }
    public required string H1 { get; init; }
    public required string Kicker { get; init; }
    public required string Lead { get; init; }
    public required IReadOnlyList<string> Benefits { get; init; }
    public required string ModuleUrl { get; init; }
    public required string ModuleLabel { get; init; }
    public IReadOnlyList<SeoFaqItem> Faqs { get; init; } = Array.Empty<SeoFaqItem>();
}

public sealed class SeoFaqItem
{
    public required string Question { get; init; }
    public required string Answer { get; init; }
}
