namespace ST10448420_CMCsystem.Models.ViewModels
{
    public class HRClaimsViewModel
    {
        public string ClaimID { get; set; }
        public string LecturerName { get; set; }
        public string ClaimName { get; set; }
        public string Status { get; set; }
        public DateTime Submitted { get; set; }
        public decimal Amount { get; set; }
    }
}
