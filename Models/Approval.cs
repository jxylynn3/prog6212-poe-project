using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace ST10448420_CMCsystem.Models
{
    public class Approval
    {
        [Key]
        [StringLength(50)]
        public string ApprovalID { get; set; }

        [Required, StringLength(10)]
        public string ClaimID { get; set; }

        [ForeignKey("ClaimID")]
        public Claims Claims { get; set; }

        public DateTime ApprovalDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string ApprovalStatus { get; set; } = "Pending";

        [StringLength(500)]
        public string AdditionalInformation { get; set; }

        [StringLength(10)]
        public string ProgrammeCoordinatorID { get; set; }

        [ForeignKey("ProgrammeCoordinatorID")]
        public ProgrammeCoordinator ProgrammeCoordinator { get; set; }

        [StringLength(10)]
        public string AcademicManagerID { get; set; }

        [ForeignKey("AcademicManagerID")]
        public AcademicManager AcademicManager { get; set; }
    }
}
