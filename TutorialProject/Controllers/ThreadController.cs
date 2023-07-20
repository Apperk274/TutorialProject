using BusinessLayer;
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
        private readonly ThreadDal _threadDal;
        private readonly ThreadService _threadService;

        public ThreadController(ILogger<ThreadController> logger, ThreadDal threadDal, ThreadService threadService)
        {
            _logger = logger;
            _threadDal = threadDal;
            _threadService = threadService;
        }

        [HttpGet]
        public JsonResult Comments(int id)
        {
            var comments = _threadDal.GetCommentsOfThread(id);
            return Json(comments);
        }

        public IActionResult List()
        {
            var threads = _threadDal.GetList();
            return View(threads);
        }

        public IActionResult Details(int id)
        {
            var threadDetails = _threadService.GetThreadDetails(id);
            return View(threadDetails);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
