using Microsoft.AspNetCore.Mvc;

namespace TutorialProject.Controllers
{
    public class AuthController : Controller
    {

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
    }
}
