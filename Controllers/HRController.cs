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
                        Password = vm.Password
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
    }
}
