namespace ST10448420_CMCsystem.Models.ViewModels
{
    public class ReportsViewModel//the purpose of this view model is to gather the data needed for generating reports on claim statuses.
    {
        public int Pending { get; set; }
        public int ReReview { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }

        public DateTime GeneratedOn { get; set; } = DateTime.Now;
    }
}
