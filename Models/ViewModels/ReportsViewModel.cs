namespace ST10448420_CMCsystem.Models.ViewModels
{
    public class ReportsViewModel
    {
        public int Pending { get; set; }
        public int ReReview { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }

        public DateTime GeneratedOn { get; set; } = DateTime.Now;
    }
}
