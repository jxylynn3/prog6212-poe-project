using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace ST10448420_CMCsystem.Models
{
    [Table("SupportingDocument")]
    public class SupportingDocx
    {
        [Key]
        [StringLength(10)]
        public string DocumentID { get; set; }

        [Required, StringLength(10)]
        public string ClaimID { get; set; }

        [ForeignKey("ClaimID")]
        public Claims Claims { get; set; }

        [Required, StringLength(255)]
        public string FileName { get; set; }

        [Required, StringLength(500)]
        public string FilePath { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string? AdditionalNotes { get; set; }
    }
}
