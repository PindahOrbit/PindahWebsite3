namespace PindahWebsite3.Services;

public class ProductGuidesOptions
{
    public const string SectionName = "ProductGuides";

    /// <summary>
    /// Source folder containing *.md, *.pdf, and screenshots/.
    /// Absolute path, or relative to the web app content root.
    /// </summary>
    public string SourcePath { get; set; } =
        "../Operations.API/Operations.API/wwwroot/branding/product-documentation";

    /// <summary>How often to re-copy when the source is reachable.</summary>
    public int SyncIntervalSeconds { get; set; } = 30;

    public bool Enabled { get; set; } = true;
}
