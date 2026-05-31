using PindahWebsite3.Models;

namespace PindahWebsite3.ViewModels
{
    public class ZimsecIndexViewModel
    {
        public List<ZimsecCategory> Categories { get; set; } = new List<ZimsecCategory>();
        public List<ZimsecDocument> RecentDocuments { get; set; } = new List<ZimsecDocument>();
        // For selecting a category in the upload form
        public List<ZimsecCategory> FlatCategories { get; set; } = new List<ZimsecCategory>();

        public string SearchQuery { get; set; } = string.Empty;
        public bool IsSearch { get; set; } = false;
        public List<ZimsecCategory> SearchCategoryResults { get; set; } = new List<ZimsecCategory>();
        public List<ZimsecDocument> SearchDocumentResults { get; set; } = new List<ZimsecDocument>();
    }
}
