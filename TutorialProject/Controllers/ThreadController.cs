using BusinessLayer;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using TutorialProject.Models;
using VoteApi;

namespace TutorialProject.Controllers
{
    public class ThreadController : Controller
    {
        private readonly ILogger<ThreadController> _logger;
        private readonly ThreadDal _threadDal;
        private readonly ThreadService _threadService;
        private readonly VoteService _voteService;

        public ThreadController(ILogger<ThreadController> logger, ThreadDal threadDal, ThreadService threadService, VoteService voteService)
        {
            _logger = logger;
            _threadDal = threadDal;
            _threadService = threadService;
            _voteService = voteService;
        }

        public IActionResult List()
        {
            var threads = _threadDal.GetList();
            return View(threads);
        }

        public IActionResult Details(int id)
        {
            var threadDetails = _threadService.GetThreadDetails(id);
            var (UpVotes, DownVotes) = _voteService.GetUpvotesAndDownvotesForThread(id);
            var threadVM = new ThreadViewModel()
            {
                Thread = threadDetails.Thread,
                NumOfComments = threadDetails.NumOfComments,
                UpVotes = UpVotes,
                DownVotes = DownVotes,
            };
            return View(threadVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
