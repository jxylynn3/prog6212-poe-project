using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10448420_CMCsystem.Data;
using ST10448420_CMCsystem.Models;

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
            var vm = new ClaimCreateViewModel
            {
                ClaimID = Guid.NewGuid().ToString().Substring(0, 10),
                LecturerID = lecturerId ?? "",
            };

            if (!string.IsNullOrEmpty(lecturerId))
            {
                var lec = _db.Lecturer.FirstOrDefault(l => l.LecturerID == lecturerId);
                if (lec != null) vm.LecturerName = $"{lec.FirstName} {lec.LastName}";
            }

            return View(vm);
        }

        // POST: submit completed claim
    [HttpPost]
    [ValidateAntiForgeryToken]
        public IActionResult Create(ClaimCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Map ViewModel → Entity
            var claim = new Claims
            {
                ClaimID = vm.ClaimID,
                LecturerID = vm.LecturerID,
                ClaimName = vm.ClaimName,
                ClaimDescription = vm.ClaimDescription,
                ClaimDate = vm.ClaimDate,
                TotalHoursWorked = vm.TotalHoursWorked,
                HourlyRate = vm.HourlyRate,
                ClaimStatus = vm.ClaimStatus ?? "Pending",
                ClaimSubmissionDate = DateTime.Now
            };

            _db.Claims.Add(claim);
            _db.SaveChanges();

            // Add uploaded files
            if (vm.UploadedFiles != null && vm.UploadedFiles.Any())
            {
                foreach (var f in vm.UploadedFiles)
                {
                    var doc = new SupportingDocx
                    {
                        DocumentID = string.IsNullOrEmpty(f.DocumentID)
                            ? Guid.NewGuid().ToString().Substring(0, 10)
                            : f.DocumentID,
                        ClaimID = claim.ClaimID,
                        FileName = f.FileName,
                        FilePath = f.FilePath,
                        UploadedDate = DateTime.Now
                    };
                    _db.SupportingDocuments.Add(doc);
                }
                _db.SaveChanges();
            }

            // Redirect back to dashboard
            return RedirectToAction("LecturerDashboard", "Dashboard", new { id = vm.LecturerID });
        }

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

            var doc = new SupportingDocx
            {
                DocumentID = Guid.NewGuid().ToString().Substring(0, 10),
                ClaimID = claimId,
                FileName = fileName,
                FilePath = $"/uploads/{claimId}/{uniqueName}",
                UploadedDate = DateTime.Now
            };

            _db.SupportingDocuments.Add(doc);
            _db.SaveChanges();

            return Json(new
            {
                success = true,
                documentId = doc.DocumentID,
                fileName = doc.FileName,
                filePath = doc.FilePath
            });
        }
    }
}
