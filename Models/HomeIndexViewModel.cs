namespace PindahWebsite3.Models;

public class HomeIndexViewModel
{
    public List<ModuleCardViewModel> Modules { get; set; } = new();
    public List<News> FeaturedNews { get; set; } = new();
}
