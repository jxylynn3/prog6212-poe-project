using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10448420_CMCsystem.Models
{
    [Table("Claim")]
    public class Claims
    {
        [Key]
        [StringLength(10)]
        public string ClaimID { get; set; }

        [Required, StringLength(10)]
        public string LecturerID { get; set; }

        [ForeignKey("LecturerID")]
        public Lecturer Lecturer { get; set; }

        [Required, StringLength(50)]
        public string ClaimName { get; set; }

        [StringLength(255)]
        public string ClaimDescription { get; set; }

        [Required]
        public DateTime ClaimDate { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "9999")]
        public decimal TotalHoursWorked { get; set; }

        // This stores the final money value
        public decimal TotalAmount { get; set; }

        [Required]
        [Range(typeof(decimal), "28.79", "9999")]
        public decimal HourlyRate { get; set; }

        [NotMapped]
        public decimal TotalSalary => TotalHoursWorked * HourlyRate;

        [StringLength(50)]
        public string ClaimStatus { get; set; } = "Pending";

        public DateTime ClaimSubmissionDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<SupportingDocx> SupportingDocuments { get; set; }
        public ICollection<Approval> Approvals { get; set; }
    }
}
