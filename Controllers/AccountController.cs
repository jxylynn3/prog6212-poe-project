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
        public IActionResult Register(UserAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
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
                        _db.Lecturers.Add(lecturer);
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
                        _db.AcademicManagers.Add(manager);
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
                        _db.ProgrammeCoordinators.Add(coordinator);
                        break;
                }

                _db.SaveChanges();
                ViewBag.Message = "Registration successful!";
                return RedirectToAction("Login");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string role)
        {
            bool isValid = false;

            switch (role)
            {
                case "Lecturer":
                    isValid = _db.Lecturers.Any(x => x.Username == username && x.Password == password);
                    if (isValid) return RedirectToAction("LecturerDashboard", "Dashboard");
                    break;

                case "AcademicManager":
                    isValid = _db.AcademicManagers.Any(x => x.Username == username && x.Password == password);
                    if (isValid) return RedirectToAction("AcademicManagerDashboard", "Dashboard");
                    break;

                case "ProgrammeCoordinator":
                    isValid = _db.ProgrammeCoordinators.Any(x => x.Username == username && x.Password == password);
                    if (isValid) return RedirectToAction("ProgrammeCoordinatorDashboard", "Dashboard");
                    break;
            }

            ViewBag.Error = "Invalid username, password, or role.";
            return View();
        }
    }
}
