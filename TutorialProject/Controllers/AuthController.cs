using BusinessLayer;
using DTOLayer.ReqDTO;
using Microsoft.AspNetCore.Mvc;

namespace TutorialProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
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

        [HttpPost]
        public IActionResult Register(RegisterReqDTO registerReqDTO)
        {
            User.
            try
            {
                var user = _authService.Register(registerReqDTO);
                LogInReqDTO loginReq = new()
                {
                    Email = user.Email,
                    Password = user.Password,
                };
                _authService.LogIn(loginReq);
                return RedirectToAction("Thread");

            }
            catch
            {

                return View();
            }
        }
    }
}
