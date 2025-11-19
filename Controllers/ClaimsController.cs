using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10448420_CMCsystem.Data;
using ST10448420_CMCsystem.Models;
using ST10448420_CMCsystem.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using ST10448420_CMCsystem.Helpers;

namespace ST10448420_CMCsystem.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AppDBContext _db;
        private readonly IWebHostEnvironment _webHost;//dependency injection for web host environment

        public ClaimsController(AppDBContext context, IWebHostEnvironment env)
        {
            _db = context;
            _webHost = env;//initialize the web host environment,for file uploads usage
        }

        [HttpGet]
        public IActionResult Create(string lecturerId)//this method allows lecturers to create new claims(edits were made for part 03)
        {
            if (HttpContext.Session.UserRole() != "Lecturer")
                return RedirectToAction("Login", "Account");

            lecturerId = HttpContext.Session.UserID();

            // fetch lecturer info
            var lecturer = _db.Lecturer.FirstOrDefault(l => l.LecturerID == lecturerId);
            if (lecturer == null)
                return Unauthorized(); // shouldn't happen but safety

            // pre-fill the VM
            var vm = new ClaimCreateViewModel
            {
                LecturerID = lecturer.LecturerID,
                LecturerName = $"{lecturer.FirstName} {lecturer.LastName}",
                HourlyRate = lecturer.HourlyRate < 28.79m ? 28.79m : lecturer.HourlyRate, // enforce minimum
                ClaimDate = DateTime.Today,
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClaimCreateViewModel vm)
        {
            if (HttpContext.Session.UserRole() != "Lecturer")
                return RedirectToAction("Login", "Account");

            vm.LecturerID = HttpContext.Session.UserID();

            if (string.IsNullOrEmpty(vm.ClaimID))
                vm.ClaimID = "CLM" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            ModelState.Remove(nameof(vm.ClaimID));

            if (!ModelState.IsValid)
                return View(vm);

            if (vm.TotalHoursWorked < 5 || vm.TotalHoursWorked > 195)
            {
                ModelState.AddModelError("TotalHoursWorked", "Total hours must be between 5 and 195.");
                return View(vm);
            }

            var lecturer = _db.Lecturer.First(l => l.LecturerID == vm.LecturerID);
            decimal hourlyRate = lecturer.HourlyRate < 28.79m ? 28.79m : lecturer.HourlyRate;

            var claim = new Claims
            {
                ClaimID = vm.ClaimID,
                LecturerID = vm.LecturerID,
                ClaimName = vm.ClaimName,
                ClaimDescription = vm.ClaimDescription,
                ClaimDate = vm.ClaimDate,
                TotalHoursWorked = vm.TotalHoursWorked,
                HourlyRate = hourlyRate,
                TotalAmount = hourlyRate * (decimal)vm.TotalHoursWorked,
                ClaimStatus = "Pending",
                ClaimSubmissionDate = DateTime.Now
            };

            _db.Claims.Add(claim);
            _db.SaveChanges();

            if (vm.UploadedFiles != null)
            {
                foreach (var f in vm.UploadedFiles)
                {
                    _db.SupportingDocument.Add(new SupportingDocx
                    {
                        DocumentID = Guid.NewGuid().ToString().Substring(0, 10),
                        ClaimID = claim.ClaimID,
                        FileName = f.FileName,
                        FilePath = f.FilePath,
                        UploadedDate = DateTime.Now
                    });
                }
                _db.SaveChanges();
            }

            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> UploadDocument([FromForm] string claimId, [FromForm] IFormFile file)
        {
            // Validate file presence
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Allowed formats
            var allowed = new[] { ".pdf", ".docx", ".doc", ".png", ".jpg", ".jpeg" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowed.Contains(ext))
                return BadRequest("File type not allowed.");

            // File size limit
            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("File too large.");

            // If no claim exists yet, assign temporary ID
            // Fix added because Sessions removed TempData flow
            if (string.IsNullOrEmpty(claimId))
            {
                claimId = "TEMP_" + Guid.NewGuid().ToString("N").Substring(0, 8);
            }

            // Directory where files will be stored
            var uploadsRoot = Path.Combine(_webHost.WebRootPath, "uploads", claimId);

            // Ensure the directory exists
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var uniqueName = $"{Guid.NewGuid().ToString().Substring(0, 8)}_{file.FileName}";
            var filePath = Path.Combine(uploadsRoot, uniqueName);

            // Save file to server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Public path for front-end
            var publicPath = $"/uploads/{claimId}/{uniqueName}";

            // Return JSON to the AJAX caller
            return Json(new
            {
                success = true,
                documentId = Guid.NewGuid().ToString().Substring(0, 10),
                fileName = file.FileName,
                filePath = publicPath,
                claimId = claimId
            });
        }

        public async Task<IActionResult> List()//this method allows lecturers to view all their submitted claims,indivdualised to the lecturer account
        {
            if (HttpContext.Session.UserRole() != "Lecturer")
                return RedirectToAction("Login", "Account");

            var lecturerId = HttpContext.Session.UserID();

            var claims = await _db.Claims
                .Where(c => c.LecturerID == lecturerId)
                .OrderByDescending(c => c.ClaimDate)
                .ToListAsync();

            return View(claims);
        }

        [HttpGet]
        public async Task<IActionResult> ClaimDetails(string id, string from)
        //the use of FROM: helps to identify where the user navigated from,allowing for context-aware rendering or navigation options in the view.
        {
            if (HttpContext.Session.UserRole() == null)
                return RedirectToAction("Login", "Account");

            var claim = await _db.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.SupportingDocuments)
                .FirstOrDefaultAsync(c => c.ClaimID == id);

            if (claim == null)
                return NotFound();

            ViewBag.FromPage = from;
            return View("~/Views/Claims/ClaimDetails.cshtml", claim);
        }
        [HttpGet]
        public async Task<IActionResult> ClaimDetailsPC(string id)
        {
            // Only Programme Coordinator may access this endpoint,access control
            if (HttpContext.Session.UserRole() != "ProgrammeCoordinator")
                return RedirectToAction("Login", "Account");

            if (id == null)
                return NotFound();

            var claim = await _db.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.SupportingDocuments)
                .FirstOrDefaultAsync(c => c.ClaimID == id);

            if (claim == null)
                return NotFound();

            // The PC view expects the claim model
            return View("~/Views/Claims/ClaimDetailsPC.cshtml", claim);
        }

        // this method allows lecturers to trace their claims,by viewing all their submitted claims
        [HttpGet]
        public async Task<IActionResult> TraceClaims()
        {
            // Enforce only lecturers can view trace,this is the access control,highlighted in the checklist
            if (HttpContext.Session.UserRole() != "Lecturer")
                return RedirectToAction("Login", "Account");

            var lecturerId = HttpContext.Session.UserID();

            // If the session somehow has no ID, force logout,this is a form of error handling.it ensures that only authenticated lecturers can access their claims
            if (string.IsNullOrEmpty(lecturerId))
                return RedirectToAction("Logout", "Account");

            var claims = await _db.Claims
                .Where(c => c.LecturerID == lecturerId)
                .OrderByDescending(c => c.ClaimDate)
                .ToListAsync();

            return View("~/Views/Claims/TraceClaims.cshtml", claims);
        }


        [HttpGet]
        public async Task<IActionResult> ReviewClaims()//used by academic manager to review claims,for the workflow of approving or rejecting claims
        {
            if (HttpContext.Session.UserRole() != "AcademicManager")
                return Unauthorized();

            var allClaims = await _db.Claims
                .Include(c => c.Lecturer)
                .ToListAsync();

            var viewModel = new ClaimsDashboardViewModel
            {
                PendingClaims = allClaims.Where(c => c.ClaimStatus == "Pending").ToList(),
                ReReviewClaims = allClaims.Where(c => c.ClaimStatus == "Approved by Programme Coordinator").ToList(),
                ApprovedClaims = allClaims.Where(c => c.ClaimStatus == "Approved by Academic Manager").ToList(),
                RejectedClaims = allClaims.Where(c => c.ClaimStatus == "Rejected by Academic Manager").ToList()
            };

            return View("~/Views/Dashboard/AcademicManagerDashboard.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ReviewClaimsPC()//used by programme coordinator to review claims,for the workflow of approving or rejecting claims
        {
            if (HttpContext.Session.UserRole() != "ProgrammeCoordinator")
                return Unauthorized();

            var claims = await _db.Claims
                .Include(c => c.Lecturer)
                .ToListAsync();

            var vm = new ClaimsDashboardViewModel
            {
                PendingClaims = claims.Where(c => c.ClaimStatus == "Pending").ToList(),
                ApprovedClaims = claims.Where(c => c.ClaimStatus == "Approved by Programme Coordinator").ToList(),
                RejectedClaims = claims.Where(c => c.ClaimStatus == "Rejected by Programme Coordinator").ToList(),
            };

            return View("~/Views/Dashboard/ProgrammeCoordinatorDashboard.cshtml", vm);
        }
        // Approve for Programme Coordinator
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveClaimsPC(string id)
        {
            if (HttpContext.Session.UserRole() != "ProgrammeCoordinator")
                return Unauthorized();

            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.ClaimStatus = "Approved by Programme Coordinator";
            _db.Update(claim);
            await _db.SaveChangesAsync();

            return RedirectToAction("ReviewClaimsPC");
        }

        // Reject for Programme Coordinator
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectClaimsPC(string id)
        {
            if (HttpContext.Session.UserRole() != "ProgrammeCoordinator")
                return Unauthorized();

            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.ClaimStatus = "Rejected by Programme Coordinator";
            _db.Update(claim);
            await _db.SaveChangesAsync();

            return RedirectToAction("ReviewClaimsPC");
        }
        // Approve for Academic Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveClaims(string id)
        {
            // Ensure only Academic Manager can call this
            if (HttpContext.Session.UserRole() != "AcademicManager")
                return Unauthorized();

            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            // Update status and save
            claim.ClaimStatus = "Approved by Academic Manager";
            _db.Update(claim);
            await _db.SaveChangesAsync();

            return RedirectToAction("ReviewClaims");
        }

        // Reject for Academic Manager
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectClaims(string id)
        {
            if (HttpContext.Session.UserRole() != "AcademicManager")
                return Unauthorized();

            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.ClaimStatus = "Rejected by Academic Manager";
            _db.Update(claim);
            await _db.SaveChangesAsync();

            return RedirectToAction("ReviewClaims");
        }


        //the method below allows HR to view all claims in the system
        [HttpGet]
        public async Task<IActionResult> HRClaims()
        {
            if (HttpContext.Session.UserRole() != "HR")
                return RedirectToAction("Login", "Account");//this checks if the user is HR

            var claims = await _db.Claims
                .Include(c => c.Lecturer)
                .OrderByDescending(c => c.ClaimSubmissionDate)
                .ToListAsync();

            return View("~/Views/HR/HRClaims.cshtml", claims);
        }
        //part 03
        [HttpPost]
        public async Task<IActionResult> DeleteClaim(string id)
        {
            if (HttpContext.Session.UserRole() != "HR")
                return Unauthorized();

            var claim = await _db.Claims
                .Include(c => c.SupportingDocuments)
                .FirstOrDefaultAsync(c => c.ClaimID == id);

            if (claim == null)
                return NotFound();

            if (claim.SupportingDocuments != null)
                _db.SupportingDocument.RemoveRange(claim.SupportingDocuments);

            _db.Claims.Remove(claim);
            await _db.SaveChangesAsync();

            return RedirectToAction("HRClaims");
        }
    }
}
