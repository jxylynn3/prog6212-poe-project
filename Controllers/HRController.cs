using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using ST10448420_CMCsystem.Data;
using ST10448420_CMCsystem.Helpers;
using ST10448420_CMCsystem.Models;
using ST10448420_CMCsystem.Models.ViewModels;

namespace ST10448420_CMCsystem.Controllers
{
    public class HRController : Controller
    {
        private readonly AppDBContext _db;

        public HRController(AppDBContext db)//property injection of the DB context
        {
            _db = db;
        }

        private bool IsHR()
        {
            //this boolean method checks if the current user is an HR,its purpose is to make the restriction of certain actions to HR users only easier
            return HttpContext.Session.UserRole() == "HR";//gets called alot in the methods below
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            //CRUD functionality for creating new users in the system,solely used by the HR
            if (!IsHR())
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(HRUserCreationViewModel vm)
        {
            if (!IsHR())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(vm);

            // Hourly rate restriction for lecturer
            if (vm.Role == "Lecturer" && vm.HourlyRate < 28.79m)
            {
                ModelState.AddModelError("HourlyRate", "Hourly rate cannot be below R28.79.");
                return View(vm);
            }

            switch (vm.Role)
            {
                case "Lecturer":
                    _db.Lecturer.Add(new Lecturer
                    {
                        LecturerID = Guid.NewGuid().ToString().Substring(0, 10),
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Email = vm.Email,
                        Username = vm.Username,
                        Password = vm.Password,
                        HourlyRate = vm.HourlyRate
                    });
                    break;

                case "AcademicManager":
                    _db.AcademicManager.Add(new AcademicManager
                    {
                        AcademicManagerID = Guid.NewGuid().ToString().Substring(0, 10),
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Email = vm.Email,
                        Username = vm.Username,
                        Password = vm.Password
                    });
                    break;

                case "ProgrammeCoordinator":
                    _db.ProgrammeCoordinator.Add(new ProgrammeCoordinator
                    {
                        CoordinatorID = Guid.NewGuid().ToString().Substring(0, 10),
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Email = vm.Email,
                        Username = vm.Username,
                        Password = vm.Password
                    });
                    break;

                default:
                    ModelState.AddModelError("", "Invalid role selected.");
                    return View(vm);
            }

            _db.SaveChanges();
            TempData["Success"] = "User created successfully!";
            return RedirectToAction("HRDashboard", "Dashboard");
        }

        [HttpGet]
        public IActionResult ManageUsers()
        {
            //solely used by the HR to manage users in the system
            if (!IsHR())
                return RedirectToAction("Login", "Account");

            var vm = new HRUserManagementViewModel
            {
                Lecturers = _db.Lecturer.ToList(),
                AcademicManagers = _db.AcademicManager.ToList(),
                ProgrammeCoordinators = _db.ProgrammeCoordinator.ToList()
            };

            return View(vm);
        }

        //this is a helper method to get user by role and id
        private object GetUserByRole(string role, string id)
        {//is this neccessay? --> yes, it is used in multiple methods below to get the user based on their role and id
            return role switch
            {
                "Lecturer" => _db.Lecturer.FirstOrDefault(x => x.LecturerID == id),
                "ProgrammeCoordinator" => _db.ProgrammeCoordinator.FirstOrDefault(x => x.CoordinatorID == id),
                "AcademicManager" => _db.AcademicManager.FirstOrDefault(x => x.AcademicManagerID == id),
                _ => null
            };
        }


        [HttpGet]
        public IActionResult UserDetails(string role, string id)
        //a method that displays user details based on their role and id
        {
            if (!IsHR())
                return RedirectToAction("Login", "Account");

            var user = GetUserByRole(role, id);
            if (user == null)
                return NotFound();

            ViewBag.Role = role;
            return View(user);
        }


        [HttpGet]
        public IActionResult EditUser(string role, string id)
        {
            //CRUD functionality for editing user details based on their role and id.This is the GET method tho
            if (!IsHR())
                return RedirectToAction("Login", "Account");

            var user = GetUserByRole(role, id);
            if (user == null)
                return NotFound();

            ViewBag.Role = role;
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(string role, string id, Lecturer lecturer, ProgrammeCoordinator coordinator, AcademicManager manager)
        {
            //the POST method is used to save the changes made to the user details to the DB
            if (!IsHR())
                return RedirectToAction("Login", "Account");

            switch (role)
            {
                case "Lecturer":
                    // Validation BEFORE database update
                    if (lecturer.HourlyRate < 28.79m)
                    {
                        ViewBag.Role = role;
                        ModelState.AddModelError("HourlyRate", "Hourly rate cannot be below R28.79.");
                        return View(lecturer);
                    }

                    var lec = _db.Lecturer.FirstOrDefault(x => x.LecturerID == id);
                    if (lec != null)
                    {
                        lec.FirstName = lecturer.FirstName;
                        lec.LastName = lecturer.LastName;
                        lec.Email = lecturer.Email;
                        lec.Username = lecturer.Username;
                        lec.HourlyRate = lecturer.HourlyRate;
                    }
                    break;

                case "ProgrammeCoordinator":
                    var pc = _db.ProgrammeCoordinator.FirstOrDefault(x => x.CoordinatorID == id);
                    if (pc != null)
                    {
                        pc.FirstName = coordinator.FirstName;
                        pc.LastName = coordinator.LastName;
                        pc.Email = coordinator.Email;
                        pc.Username = coordinator.Username;
                    }
                    break;

                case "AcademicManager":
                    var am = _db.AcademicManager.FirstOrDefault(x => x.AcademicManagerID == id);
                    if (am != null)
                    {
                        am.FirstName = manager.FirstName;
                        am.LastName = manager.LastName;
                        am.Email = manager.Email;
                        am.Username = manager.Username;
                    }
                    break;
            }

            _db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }


        [HttpGet]
        public IActionResult DeleteUser(string role, string id)
        {
            //CRUD functionality for deleting a user based on their role and id
            if (!IsHR())
                return RedirectToAction("Login", "Account");

            var user = GetUserByRole(role, id);
            if (user == null)
                return NotFound();

            ViewBag.Role = role;
            ViewBag.Id = id;
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteUserConfirmed(string role, string id)
        {
            if (!IsHR())
                return RedirectToAction("Login", "Account");

            switch (role)
            {
                case "Lecturer":
                    var lec = _db.Lecturer.FirstOrDefault(x => x.LecturerID == id);
                    if (lec != null) _db.Lecturer.Remove(lec);
                    break;

                case "ProgrammeCoordinator":
                    var pc = _db.ProgrammeCoordinator.FirstOrDefault(x => x.CoordinatorID == id);
                    if (pc != null) _db.ProgrammeCoordinator.Remove(pc);
                    break;

                case "AcademicManager":
                    var am = _db.AcademicManager.FirstOrDefault(x => x.AcademicManagerID == id);
                    if (am != null) _db.AcademicManager.Remove(am);
                    break;
            }

            _db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }

        [HttpGet]
        public IActionResult Reports()
        {//a method that generates reports for HR users,using a view model to pass the data to the view
            if (!IsHR())
                return RedirectToAction("Login", "Account");

            var vm = new ReportsViewModel
            {
                Pending = _db.Claims.Count(c => c.ClaimStatus == "Pending"),
                ReReview = _db.Claims.Count(c => c.ClaimStatus == "Approved by Programme Coordinator"),
                Approved = _db.Claims.Count(c => c.ClaimStatus == "Approved by Academic Manager"),
                Rejected = _db.Claims.Count(c => c.ClaimStatus.Contains("Rejected"))
            };

            return View("~/Views/HR/Reports.cshtml", vm);
        }
        //we are gonna use itextsharp for pdf generation in the reports section in the future
        [HttpGet]
        public IActionResult DownloadReport()
        {
            if (!IsHR())
                return RedirectToAction("Login", "Account");

            var vm = new ReportsViewModel
            {
                Pending = _db.Claims.Count(c => c.ClaimStatus == "Pending"),
                ReReview = _db.Claims.Count(c => c.ClaimStatus == "Approved by Programme Coordinator"),
                Approved = _db.Claims.Count(c => c.ClaimStatus == "Approved by Academic Manager"),
                Rejected = _db.Claims.Count(c => c.ClaimStatus.Contains("Rejected")),
                GeneratedOn = DateTime.Now
            };

            using (var ms = new MemoryStream())
            {
                var doc = new iTextSharp.text.Document();
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                doc.Add(new Paragraph("CMC System – HR Claims Report"));
                doc.Add(new Paragraph("-----------------------------------------"));
                doc.Add(new Paragraph($"Generated On: {vm.GeneratedOn}"));
                doc.Add(new Paragraph(""));
                doc.Add(new Paragraph($"Pending Claims: {vm.Pending}"));
                doc.Add(new Paragraph($"Up for Re-Review (PC Approved): {vm.ReReview}"));
                doc.Add(new Paragraph($"Approved by Academic Manager: {vm.Approved}"));
                doc.Add(new Paragraph($"Rejected Claims: {vm.Rejected}"));

                doc.Close();

                return File(ms.ToArray(), "application/pdf", "HR_Claims_Report.pdf");
            }
        }

    }
}
