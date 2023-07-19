using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TutorialProject.Models;

namespace TutorialProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly ThreadDal _threadDal;

        public HomeController(ILogger<HomeController> logger, ThreadDal threadDal)
        {
            _logger = logger;
            _threadDal = threadDal;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Thread(int id)
        {
            var thread = _threadDal.Get(id);
            return View(thread);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
