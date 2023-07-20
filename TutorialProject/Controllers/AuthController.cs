using BusinessLayer;
using DTOLayer.ReqDTO;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace TutorialProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        public async Task<IActionResult> LogIn(LogInReqDTO logInReqDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(logInReqDTO.Email, logInReqDTO.Password, false, true);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Thread");
                }

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterReqDTO registerReqDTO)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Email = registerReqDTO.Email,
                    UserName = registerReqDTO.Email,
                    Name = registerReqDTO.Name,
                    Surname = registerReqDTO.Surname,
                };

                var result = await _userManager.CreateAsync(user, registerReqDTO.Password);
                Console.Write(result.ToString());
                if (result.Succeeded)
                {
                    return RedirectToAction("LogIn", "Auth");
                }

            }
            return View();
        }
    }
}
