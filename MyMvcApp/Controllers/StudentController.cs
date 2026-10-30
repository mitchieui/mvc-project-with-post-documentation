using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            Student stud = new Student();
            stud.FirstName = "John";
            stud.MiddleName = "Blake";
            stud.LastName = "Doe";
            stud.StudentNumber = "2020-12312-MN-0";

            ViewBag.KahitAnongTitle = "Student Information";
            ViewData["KahitAnoUlet"] = "Second Title";

            return View(stud);
        }
    }
}
