using Microsoft.AspNetCore.Mvc;
using ST10448420_CMCsystem.Data;
using ST10448420_CMCsystem.Models;

namespace ST10448420_CMCsystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDBContext _db;

        public AccountController(AppDBContext db)
        {
            _db = db;
        }

        // --- REGISTER ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // simple password length check (no regex)
            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 8)
            {
                ModelState.AddModelError("Password", "Password must be at least 8 characters.");
                return View(model);
            }

            // Check duplicates across tables (username and email)
            bool usernameExists = _db.Lecturer.Any(x => x.Username == model.Username)
                               || _db.AcademicManager.Any(x => x.Username == model.Username)
                               || _db.ProgrammeCoordinator.Any(x => x.Username == model.Username);

            if (usernameExists)
            {
                ModelState.AddModelError("Username", "Username already taken.");
                return View(model);
            }

            bool emailExists = _db.Lecturer.Any(x => x.Email == model.Email)
                            || _db.AcademicManager.Any(x => x.Email == model.Email)
                            || _db.ProgrammeCoordinator.Any(x => x.Email == model.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email already in use.");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Role))
            {
                ModelState.AddModelError("Role", "Please select a role.");
                return View(model);
            }

            switch (model.Role)
            {
                case "Lecturer":
                    var lecturer = new Lecturer
                    {
                        LecturerID = Guid.NewGuid().ToString().Substring(0, 10),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Username = model.Username,
                        Password = model.Password
                    };
                    _db.Lecturer.Add(lecturer);
                    break;

                case "AcademicManager":
                    var manager = new AcademicManager
                    {
                        AcademicManagerID = Guid.NewGuid().ToString().Substring(0, 10),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Username = model.Username,
                        Password = model.Password
                    };
                    _db.AcademicManager.Add(manager);
                    break;

                case "ProgrammeCoordinator":
                    var coordinator = new ProgrammeCoordinator
                    {
                        CoordinatorID = Guid.NewGuid().ToString().Substring(0, 10),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Username = model.Username,
                        Password = model.Password
                    };
                    _db.ProgrammeCoordinator.Add(coordinator);
                    break;

                default:
                    ModelState.AddModelError("Role", "Unknown role.");
                    return View(model);
            }

            _db.SaveChanges();

            // Provide a friendly success message (optional) and redirect to Login
            TempData["RegisterSuccess"] = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                ViewBag.Error = "Please provide username, password and role.";
                return View();
            }

            bool isValid = false;

            switch (role)
            {
                case "Lecturer":
                    var lecturer = _db.Lecturer.FirstOrDefault(x => x.Username == username && x.Password == password);
                    if (lecturer != null)
                    {
                        // Store lecturer details for later
                        TempData["LecturerID"] = lecturer.LecturerID;
                        TempData["LecturerName"] = $"{lecturer.FirstName} {lecturer.LastName}";
                        return RedirectToAction("LecturerDashboard", "Dashboard", new { lecturerId = lecturer.LecturerID });
                    }
                    break;

                case "AcademicManager":
                    isValid = _db.AcademicManager.Any(x => x.Username == username && x.Password == password);
                    if (isValid) return RedirectToAction("AcademicManagerDashboard", "Dashboard", new { username = username });
                    break;

                case "ProgrammeCoordinator":
                    isValid = _db.ProgrammeCoordinator.Any(x => x.Username == username && x.Password == password);
                    if (isValid) return RedirectToAction("ProgrammeCoordinatorDashboard", "Dashboard", new { username = username });
                    break;
                //Case for HR role can be added here in future
                case "HR":
                    var hr = _db.HR.FirstOrDefault(x => x.Username == username && x.Password == password);
                    if (hr != null)
                    {
                        TempData["HRID"] = hr.HRID;
                        TempData["HRName"] = $"{hr.FirstName} {hr.Surname}";
                        return RedirectToAction("HRDashboard", "Dashboard");
                    }
                    break;

                default:
                    ViewBag.Error = "Unknown role.";
                    return View();
            }

            ViewBag.Error = "Invalid username, password, or role.";
            return View();
        }
    }
}
