using Microsoft.AspNetCore.Mvc;
using ST10448420_CMCsystem.Data;

namespace ST10448420_CMCsystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDBContext _db;
        public DashboardController(AppDBContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult LecturerDashboard(string lecturerId)
        {
            // Get lecturerId from TempData if not passed in
            if (string.IsNullOrEmpty(lecturerId))
            {
                lecturerId = TempData["LecturerID"]?.ToString();
            }

            ViewBag.LecturerID = lecturerId;
            ViewBag.LecturerName = TempData["LecturerName"]?.ToString();

            // If lecturerId still missing, fallback to default (for testing)
            if (string.IsNullOrEmpty(lecturerId))
                lecturerId = "L001"; // <- optional fallback for testing only

            // Fetch recent claims for this lecturer
            var recentClaims = _db.Claims
                .Where(c => c.LecturerID == lecturerId)
                .OrderByDescending(c => c.ClaimDate)
                .Take(3)
                .ToList();

            ViewBag.RecentClaims = recentClaims;

            return View();
        }

        [HttpGet]
        public IActionResult AcademicManagerDashboard()
        {
            return RedirectToAction("ReviewClaims", "Claims");
        }


        [HttpGet]
        public IActionResult ProgrammeCoordinatorDashboard()
        {
            return RedirectToAction("ReviewClaimsPC", "Claims");
        }
        //part 03
        //this method ensures that when HR logs in, they are directed to the HR dashboard view
        [HttpGet]
        public IActionResult HRDashboard()
        {
            return View();
        }
    }
}

