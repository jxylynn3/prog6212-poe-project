using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ST10448420_CMCsystem.Models
{
    public class Lecturer
    {
        [Key]
        [StringLength(10)]
        public string LecturerID { get; set; }

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
        public ICollection<Claims> Claims { get; set; }
    }
}
