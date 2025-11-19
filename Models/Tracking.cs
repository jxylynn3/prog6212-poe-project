using System.ComponentModel.DataAnnotations;

namespace ST10448420_CMCsystem.Models
{
    public class Tracking
    {
        [Key] public int TrackingId { get; set; }
        [Required] public string UserName { get; set; } // Username of the user who performed the action [Required] public string ActionType { get; set; } // Examples: // "Created Claim", "Edited Claim", "Approved Claim", // "Rejected Claim", "Logged In", "Deleted User", etc. [Required] public DateTime Timestamp { get; set; } = DateTime.Now; [Required] public string RecordAffected { get; set; } // e.g. "Claim #CLA123", "Lecturer #LECT001", or "N/A" // Optional: store user role if you wanna expand reporting later public string UserRole { get; set; }
    }
}
