using Microsoft.AspNetCore.Mvc;
using ST10448420_CMCsystem.Data;
using ST10448420_CMCsystem.Helpers;// for session extension methods

namespace ST10448420_CMCsystem.Controllers
{
    //this class basically handles the different dashboards for each user role in the system,this is just for better organization of the code
    public class DashboardController : Controller
    {
        private readonly AppDBContext _db;

        public DashboardController(AppDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult LecturerDashboard()
        {
            if (HttpContext.Session.UserRole() != "Lecturer")
                return RedirectToAction("Login", "Account");

            var userId = HttpContext.Session.UserID();

            var recentClaims = _db.Claims
                .Where(c => c.LecturerID == userId)
                .OrderByDescending(c => c.ClaimDate)
                .Take(3)
                .ToList();

            ViewBag.LecturerID = userId;
            ViewBag.LecturerName = HttpContext.Session.UserName();
            ViewBag.RecentClaims = recentClaims;

            return View();
        }

        [HttpGet]
        public IActionResult AcademicManagerDashboard()
        {
            if (HttpContext.Session.UserRole() != "AcademicManager")
                return RedirectToAction("Login", "Account");

            return RedirectToAction("ReviewClaims", "Claims");
        }

        [HttpGet]
        public IActionResult ProgrammeCoordinatorDashboard()
        {
            if (HttpContext.Session.UserRole() != "ProgrammeCoordinator")
                return RedirectToAction("Login", "Account");

            return RedirectToAction("ReviewClaimsPC", "Claims");
        }

        [HttpGet]
        public IActionResult HRDashboard()
        {
            if (HttpContext.Session.UserRole() != "HR")
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}
