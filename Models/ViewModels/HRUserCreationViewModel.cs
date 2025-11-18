namespace ST10448420_CMCsystem.Models.ViewModels
{
    public class HRUserCreationViewModel
    {
        public string Role { get; set; }  // Lecturer, AcademicManager, ProgrammeCoordinator
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal HourlyRate { get; set; } // For lecturers
    }
}
