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
        [Range(0.01, double.MaxValue)]
        public double TotalHoursWorked { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public double HourlyRate { get; set; }

        [NotMapped]
        public double TotalSalary => TotalHoursWorked * HourlyRate;

        [StringLength(50)]
        public string ClaimStatus { get; set; } = "Pending";

        public DateTime ClaimSubmissionDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<SupportingDocx> SupportingDocuments { get; set; }
        public ICollection<Approval> Approvals { get; set; }
    }
}
