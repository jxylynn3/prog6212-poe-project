namespace ST10448420_CMCsystem.Models.ViewModels
{
    public interface IClaimsDashboardViewModel
    {
        List<Claims> PendingClaims { get; set; }
        List<Claims> ApprovedClaims { get; set; }
        List<Claims> ReReviewClaims { get; set; }
        List<Claims> RejectedClaims { get; set; }
    }

    public class ClaimsDashboardViewModel : IClaimsDashboardViewModel
    {
        public List<Claims> PendingClaims { get; set; } = new();
        public List<Claims> ApprovedClaims { get; set; } = new();
        public List<Claims> ReReviewClaims { get; set; } = new();
        public List<Claims> RejectedClaims { get; set; } = new();
    }
}

