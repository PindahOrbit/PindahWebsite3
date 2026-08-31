namespace PindahWebsite3.Models;

public class ModuleCardViewModel
{
    public string IconUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LinkText { get; set; } = string.Empty;
    public string? Controller { get; set; }
    public string? Action { get; set; } = "Index";
    public string? Href { get; set; }
    public string? LinkRel { get; set; }
}
