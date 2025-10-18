using System.ComponentModel.DataAnnotations;

namespace ST10448420_CMCsystem.Models
{
    public class UserAccountViewModel
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
