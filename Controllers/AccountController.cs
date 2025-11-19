using Microsoft.AspNetCore.Mvc;
using ST10448420_CMCsystem.Data;
using ST10448420_CMCsystem.Models;
using Microsoft.AspNetCore.Http;


namespace ST10448420_CMCsystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDBContext _db;

        public AccountController(AppDBContext db)
        {
            _db = db;
        }

        //the register method allows new users to create accounts by providing necessary details and selecting a role,this isnt used in part 03 of the poe
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserAccountViewModel model) //not used in part 03 of the poe
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

            switch (model.Role)// pre-part 03 poe registration only for Lecturer, Academic Manager and Programme Coordinator
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
        [ValidateAntiForgeryToken]//changes were made here to accomodate the 4 roles in the poe part 03
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
                        HttpContext.Session.SetString("UserID", lecturer.LecturerID);
                        HttpContext.Session.SetString("UserRole", "Lecturer");
                        HttpContext.Session.SetString("UserName", $"{lecturer.FirstName} {lecturer.LastName}");

                        return RedirectToAction("LecturerDashboard", "Dashboard", new { lecturerId = lecturer.LecturerID });
                    }
                    break;


                case "AcademicManager":
                    var manager = _db.AcademicManager.FirstOrDefault(x => x.Username == username && x.Password == password);
                    if (manager != null)
                    {
                        HttpContext.Session.SetString("UserID", manager.AcademicManagerID);
                        HttpContext.Session.SetString("UserRole", "AcademicManager");
                        HttpContext.Session.SetString("UserName", $"{manager.FirstName} {manager.LastName}");

                        return RedirectToAction("AcademicManagerDashboard", "Dashboard");
                    }
                    break;


                case "ProgrammeCoordinator":
                    var pc = _db.ProgrammeCoordinator.FirstOrDefault(x => x.Username == username && x.Password == password);
                    if (pc != null)
                    {
                        HttpContext.Session.SetString("UserID", pc.CoordinatorID);
                        HttpContext.Session.SetString("UserRole", "ProgrammeCoordinator");
                        HttpContext.Session.SetString("UserName", $"{pc.FirstName} {pc.LastName}");

                        return RedirectToAction("ProgrammeCoordinatorDashboard", "Dashboard");
                    }
                    break;
                // the addition of the HR role login functionality,that is now used to authenticate HR users,and create other users
                case "HR":
                    var hr = _db.HR.FirstOrDefault(x => x.Username == username && x.Password == password);
                    if (hr != null)
                    {
                        HttpContext.Session.SetString("UserID", hr.HRID);//
                        HttpContext.Session.SetString("UserRole", "HR");
                        HttpContext.Session.SetString("UserName", $"{hr.FirstName} {hr.Surname}");

                        return RedirectToAction("HRDashboard", "Dashboard");
                    }
                    break;


                default: //the default case is used to handle any unknown roles that do not match the predefined ones
                    ViewBag.Error = "Unknown role.";
                    return View();
            }

            ViewBag.Error = "Invalid username, password, or role.";
            return View();
        }
        public IActionResult Logout()//the purpose of this method is to log out the user by clearing their session data and redirecting them to the login page
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
