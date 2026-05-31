using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PindahWebsite3.Models
{
    public class ZimsecCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        // Self-referencing relationship
        public int? ParentCategoryId { get; set; }
        
        [ForeignKey("ParentCategoryId")]
        public virtual ZimsecCategory? ParentCategory { get; set; }

        public virtual ICollection<ZimsecCategory> SubCategories { get; set; } = new List<ZimsecCategory>();

        public virtual ICollection<ZimsecDocument> Documents { get; set; } = new List<ZimsecDocument>();
    }
}
