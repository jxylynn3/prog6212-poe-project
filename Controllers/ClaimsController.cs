using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10448420_CMCsystem.Data;
using ST10448420_CMCsystem.Models;
using ST10448420_CMCsystem.Models.ViewModels;

namespace ST10448420_CMCsystem.Controllers
{
    public class ClaimsController : Controller
    {
    private readonly AppDBContext _db;
    private readonly IWebHostEnvironment _webHost;

    public ClaimsController(AppDBContext context, IWebHostEnvironment env)
    {
        _db = context;
        _webHost = env;
    }

        // GET: Create claim form
        [HttpGet]
        public IActionResult Create(string lecturerId)
        {
            lecturerId ??= TempData["LecturerID"]?.ToString();

            var vm = new ClaimCreateViewModel
            {
                //ClaimID = Guid.NewGuid().ToString().Substring(0, 10),
                LecturerID = lecturerId ?? "",
            };

            if (!string.IsNullOrEmpty(lecturerId))
            {
                var lec = _db.Lecturer.FirstOrDefault(l => l.LecturerID == lecturerId);
                if (lec != null)
                {
                    vm.LecturerName = $"{lec.FirstName} {lec.LastName}";
                }
            }

            return View(vm);
        }
        // the purpose of this commented-out code is to test routing only,i wasnt done with the complete implementation
        //public IActionResult Create(string lecturerId)
        //{
        //    return Content($"Create action hit with lecturerId={lecturerId}");
        //}

        // POST: submit completed claim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClaimCreateViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.ClaimID))
            {
                vm.ClaimID = "CLM" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            }

            ModelState.Remove(nameof(vm.ClaimID));

            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                Console.WriteLine("ModelState invalid: " + errors);
                return View(vm);
            }

            //this allows us to validate the total hours worked field to ensure it falls within the specified range
            if (vm.TotalHoursWorked < 5 || vm.TotalHoursWorked > 195)
            {
                ModelState.AddModelError("TotalHoursWorked", "Total hours must be between 5 and 195 hours per month.");
                return View(vm);
            }

            // optional: ensure lecturer exists
            if (string.IsNullOrEmpty(vm.LecturerID))
            {
                vm.LecturerID = TempData["LecturerID"]?.ToString();
            }

            if (string.IsNullOrEmpty(vm.LecturerName))
            {
                var lec = _db.Lecturer.FirstOrDefault(l => l.LecturerID == vm.LecturerID);
                if (lec != null)
                {
                    vm.LecturerName = $"{lec.FirstName} {lec.LastName}";
                }
            }

            // fetch lecturer to get hourly rate
            var lecturer = _db.Lecturer.First(l => l.LecturerID == vm.LecturerID);

            decimal hourlyRate = lecturer.HourlyRate;

            // Minimum enforcement
            if (hourlyRate < 28.79m)
                hourlyRate = 28.79m;

            var claim = new Claims
            {
                ClaimID = vm.ClaimID,
                LecturerID = vm.LecturerID,
                ClaimName = vm.ClaimName,
                ClaimDescription = vm.ClaimDescription,
                ClaimDate = vm.ClaimDate,

                TotalHoursWorked = vm.TotalHoursWorked, // double 
                HourlyRate = hourlyRate,                // decimal

                // TOTAL AMOUNT MUST BE DECIMAL × DOUBLE -> CAST THE DOUBLE
                TotalAmount = hourlyRate * (decimal)vm.TotalHoursWorked,

                ClaimStatus = "Pending",
                ClaimSubmissionDate = DateTime.Now
            };


            _db.Claims.Add(claim);
            _db.SaveChanges();
            Console.WriteLine($"Claim saved: {claim.ClaimID}");

            // persisted uploaded files  
            if (vm.UploadedFiles != null && vm.UploadedFiles.Any())
            {
                foreach (var f in vm.UploadedFiles)
                {
                    var doc = new SupportingDocx
                    {
                        DocumentID = string.IsNullOrEmpty(f.DocumentID) ? Guid.NewGuid().ToString().Substring(0, 10) : f.DocumentID,
                        ClaimID = claim.ClaimID,
                        FileName = f.FileName,
                        FilePath = f.FilePath,
                        UploadedDate = DateTime.Now
                    };
                    _db.SupportingDocument.Add(doc);
                }
                _db.SaveChanges();
            }

            return RedirectToAction("List", new { lecturerId = vm.LecturerID });
        }
        // I redirect to Claims.List so user sees the new claim immediately in My Claims


        // AJAX: upload file and immediately save it to server & return JSON info
        [HttpPost]
        public async Task<IActionResult> UploadDocument([FromForm] string claimId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var allowed = new[] { ".pdf", ".docx", ".doc", ".png", ".jpg", ".jpeg" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowed.Contains(ext))
                return BadRequest("File type not allowed.");

            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("File too large.");

            // Handle missing claimId gracefully
            if (string.IsNullOrEmpty(claimId))
            {
                claimId = "temp_" + Guid.NewGuid().ToString("N").Substring(0, 6);
            }

            // Directory: wwwroot/uploads/{claimId}
            var uploadsRoot = Path.Combine(_webHost.WebRootPath, "uploads", claimId);
            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var fileName = Path.GetFileName(file.FileName);
            var uniqueName = $"{Guid.NewGuid().ToString().Substring(0, 8)}_{fileName}";
            var filePath = Path.Combine(uploadsRoot, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var publicPath = $"/uploads/{claimId}/{uniqueName}";

            // Return file metadata to client
            return Json(new
            {
                success = true,
                documentId = Guid.NewGuid().ToString().Substring(0, 10),
                fileName = fileName,
                filePath = publicPath,
                claimId = claimId //include this so front-end can store it for later
            });
        }


        // GET: Claims/List?lecturerId=L001
        public async Task<IActionResult> List(string lecturerId)
        {
            if (string.IsNullOrEmpty(lecturerId))
            {
                lecturerId = "L001"; // fallback
            }

            ViewBag.LecturerID = lecturerId;

            var claims = await _db.Claims
                .Where(c => c.LecturerID == lecturerId)
                .OrderByDescending(c => c.ClaimDate)
                .ToListAsync();

            return View(claims);

        }
        //everything below focuses on the the approval/rejection process used by academic mangers
        [HttpGet]
        //views all pending claims for academic manager to review
        public async Task<IActionResult> ReviewClaims()
        {
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
        //allows for a detailed view of a specific claim
        [HttpGet]
        public async Task<IActionResult> ClaimDetails(string id)
        {
            if (id == null)
                return NotFound();

            var claim = await _db.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.SupportingDocuments)
                .FirstOrDefaultAsync(c => c.ClaimID == id);

            if (claim == null)
                return NotFound();

            return View("~/Views/Claims/ClaimDetails.cshtml", claim);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveClaims(string id)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            // If it's coming from PC approval, this is final
            if (claim.ClaimStatus == "Approved by Programme Coordinator")
                claim.ClaimStatus = "Approved by Academic Manager";
            else
                claim.ClaimStatus = "Approved";

            _db.Update(claim);
            await _db.SaveChangesAsync();

            return RedirectToAction("ReviewClaims");
        }

        [HttpPost]
        public async Task<IActionResult> RejectClaims(string id)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            // Differentiate who rejected
            if (claim.ClaimStatus == "Approved by Programme Coordinator")
                claim.ClaimStatus = "Rejected by Academic Manager";
            else
                claim.ClaimStatus = "Rejected";

            _db.Update(claim);
            await _db.SaveChangesAsync();

            return RedirectToAction("ReviewClaims");
        }

        //everything below focuses on the approval/rejection process used by Programme Coordinators

        [HttpGet]
        public async Task<IActionResult> ReviewClaimsPC()
        {
            var allClaims = await _db.Claims
                .Include(c => c.Lecturer)
                .ToListAsync();

            var viewModel = new ClaimsDashboardViewModel
            {
                PendingClaims = allClaims.Where(c => c.ClaimStatus == "Pending").ToList(),
                ApprovedClaims = allClaims.Where(c => c.ClaimStatus == "Approved by Programme Coordinator").ToList(),
                RejectedClaims = allClaims.Where(c => c.ClaimStatus == "Rejected by Programme Coordinator").ToList()

            };

            return View("~/Views/Dashboard/ProgrammeCoordinatorDashboard.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ClaimDetailsPC(string id)
        {
            if (id == null)
                return NotFound();

            var claim = await _db.Claims
                .Include(c => c.Lecturer)
                .Include(c => c.SupportingDocuments)
                .FirstOrDefaultAsync(c => c.ClaimID == id);

            if (claim == null)
                return NotFound();

            return View("~/Views/Claims/ClaimDetailsPC.cshtml", claim);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveClaimsPC(string id)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.ClaimStatus = "Approved by Programme Coordinator";
            _db.Update(claim);
            await _db.SaveChangesAsync();

            return RedirectToAction("ReviewClaimsPC");
        }

        [HttpPost]
        public async Task<IActionResult> RejectClaimsPC(string id)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.ClaimStatus = "Rejected by Programme Coordinator";
            _db.Update(claim);
            await _db.SaveChangesAsync();

            return RedirectToAction("ReviewClaimsPC");
        }
        //this action allows lecturers to trace their claims,so that they can see the status of their submissions
        [HttpGet]
        public async Task<IActionResult> TraceClaims(string lecturerId)
        {
            if (string.IsNullOrEmpty(lecturerId))
            {
                lecturerId = "L001"; // fallback or grab from session
            }

            var claims = await _db.Claims
                .Where(c => c.LecturerID == lecturerId)
                .OrderByDescending(c => c.ClaimDate)
                .ToListAsync();

            return View("~/Views/Claims/TraceClaims.cshtml", claims);
        }

    }
}
