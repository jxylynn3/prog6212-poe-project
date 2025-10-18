using Microsoft.AspNetCore.Mvc;

namespace ST10448420_CMCsystem.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult LecturerDashboard() => View();
        public IActionResult AcademicManagerDashboard() => View();
        public IActionResult ProgrammeCoordinatorDashboard() => View();
    }
}
