using System.ComponentModel.DataAnnotations;

namespace ST10448420_CMCsystem.Models
{
    public class ProgrammeCoordinator
    {
        [Key]
        [StringLength(10)]
        public string CoordinatorID { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, StringLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(50)]
        public string Password { get; set; }

        // Navigation Property
        public ICollection<Approval> Approvals { get; set; }
    }
}
