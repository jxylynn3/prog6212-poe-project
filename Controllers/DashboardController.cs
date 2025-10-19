using Microsoft.AspNetCore.Mvc;

namespace ST10448420_CMCsystem.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult LecturerDashboard(string username)
        {
            ViewBag.Username = username; // may be null if user navigates directly
            return View();
        }

        [HttpGet]
        public IActionResult AcademicManagerDashboard(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        [HttpGet]
        public IActionResult ProgrammeCoordinatorDashboard(string username)
        {
            ViewBag.Username = username;
            return View();
        }
    }
}
