using Microsoft.AspNetCore.Mvc;
using ST10448420_CMCsystem.Data;
using ST10448420_CMCsystem.Models;
using ST10448420_CMCsystem.Models.ViewModels;

namespace ST10448420_CMCsystem.Controllers
{

    public class HRController : Controller
    {

        private readonly AppDBContext _db;

        public HRController(AppDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(HRUserCreationViewModel vm)
        { 
       
        if (!ModelState.IsValid)
                return View(vm);
            if (vm.Role == "Lecturer")
            { 
            if (vm.HourlyRate < 28.79m)
                {
                    ModelState.AddModelError("HourlyRate", "Hourly rate cannot be less than the government minimum of R28.79.");
                    return View(vm);
                }
            }
            //the switch is used to determine which type of user to create based on the selected role
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
                    ModelState.AddModelError("", "Invalid role selected");
                    return View(vm);
            }

            _db.SaveChanges();
            TempData["Success"] = "User created successfully!";
            return RedirectToAction("HRDashboard", "Dashboard");
        }
        [HttpGet]
        public IActionResult ManageUsers()// this method retrieves all users from the database and displays them in the HR user management view
        { 
        var vm = new HRUserManagementViewModel
            {
                Lecturers = _db.Lecturer.ToList(),
                AcademicManagers = _db.AcademicManager.ToList(),
                ProgrammeCoordinators = _db.ProgrammeCoordinator.ToList()
            };
            return View(vm);
        }

        //An object is a instance of a class. It is created using the new keyword followed by the class constructor.
        private object GetUserByRole(string role, string id)
        {
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
        {
            var user = GetUserByRole(role, id);
            if (user == null)
                return NotFound();

            ViewBag.Role = role;
            return View(user);
        }
        [HttpGet]
        public IActionResult EditUser(string role, string id)
        {
            var user = GetUserByRole(role, id);
            if (user == null) return NotFound();

            ViewBag.Role = role;
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(string role, string id, Lecturer lecturer, ProgrammeCoordinator coordinator, AcademicManager manager)
        {
            switch (role)
            {
                case "Lecturer":
                    var lec = _db.Lecturer.FirstOrDefault(x => x.LecturerID == id);
                    if (lec != null)
                    {
                        lec.FirstName = lecturer.FirstName;
                        lec.LastName = lecturer.LastName;
                        lec.Email = lecturer.Email;
                        lec.Username = lecturer.Username;
                    }
                    break;

                case "ProgrammeCoordinator":
                    var c = _db.ProgrammeCoordinator.FirstOrDefault(x => x.CoordinatorID == id);
                    if (c != null)
                    {
                        c.FirstName = coordinator.FirstName;
                        c.LastName = coordinator.LastName;
                        c.Email = coordinator.Email;
                        c.Username = coordinator.Username;
                    }
                    break;

                case "AcademicManager":
                    var m = _db.AcademicManager.FirstOrDefault(x => x.AcademicManagerID == id);
                    if (m != null)
                    {
                        m.FirstName = manager.FirstName;
                        m.LastName = manager.LastName;
                        m.Email = manager.Email;
                        m.Username = manager.Username;
                    }
                    break;
            }

            _db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }
        [HttpGet]
        public IActionResult DeleteUser(string role, string id)
        {
            var user = GetUserByRole(role, id);
            if (user == null) return NotFound();

            ViewBag.Role = role;
            ViewBag.Id = id;
            return View(user);
        }

        [HttpPost]
        public IActionResult DeleteUserConfirmed(string role, string id)
        {
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
                    var m = _db.AcademicManager.FirstOrDefault(x => x.AcademicManagerID == id);
                    if (m != null) _db.AcademicManager.Remove(m);
                    break;
            }

            _db.SaveChanges();
            return RedirectToAction("ManageUsers");
        }

    }
}
