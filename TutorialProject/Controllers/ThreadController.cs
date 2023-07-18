using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TutorialProject.Models;

namespace TutorialProject.Controllers
{
    public class ThreadController : Controller
    {
        private readonly ILogger<ThreadController> _logger;
        readonly ThreadDal threadDal = new();

        public ThreadController(ILogger<ThreadController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int id)
        {
            var thread = threadDal.Get(id);
            return View(thread);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
