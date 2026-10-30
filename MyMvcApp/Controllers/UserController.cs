using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers
{
    public class UserController : Controller
    {
        // 1. GET: /User/Create
        // Displays the initial blank form
        [HttpGet] 
        public IActionResult Create(int id, string name)
        {
            return View();
        }

        // 2. POST: /User/Create
        // Handles the form data submitted by the user
        [HttpPost]
        public IActionResult Create(UserModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save data to database or process it here
                
                // Redirect to a success page or index to prevent duplicate submissions
                return RedirectToAction("Success", model); 
            }

            // If something failed, redisplay the form with validation errors
            return View(model);
        }

        public IActionResult Success(UserModel model)
        {
            return View(model);
        }
    }
}
