using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PindahWebsite3.Models
{
    public class ZimsecDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        // The text extracted from the PDF
        public string ExtractedText { get; set; } = string.Empty;

        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        public int? CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual ZimsecCategory? Category { get; set; }

        // Track which user uploaded the document
        public string? UploadedByUserId { get; set; }
    }
}
