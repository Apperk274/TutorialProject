using BusinessLayer;
using BusinessLayer.ValidationRules;
using DTOLayer.ReqDTO;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
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
            var validator = new LogInValidator();
            var results = validator.Validate(logInReqDTO);
            if (results.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(logInReqDTO.Email, logInReqDTO.Password, false, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Thread");
                }
            }
            else
            {
                AddErrorsToModelState(ModelState, results.Errors);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterReqDTO registerReqDTO)
        {
            var validator = new RegisterValidator();
            var results = validator.Validate(registerReqDTO);
            if (results.IsValid)
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
            else
            {
                AddErrorsToModelState(ModelState, results.Errors);
            }

            return View();
        }

        private void AddErrorsToModelState(ModelStateDictionary modelState, List<ValidationFailure> errors)
        {
            foreach (var item in errors)
            {
                modelState.AddModelError(item.PropertyName, item.ErrorMessage);
            }
        }
    }
}
