using BusinessLayer;
using DataAccessLayer.Repositories;
using DTOLayer.ReqDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Security.Claims;
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

        [HttpGet]
        public JsonResult Comments(int id)
        {
            var comments = _threadDal.GetCommentsOfThread(id);
            return Json(comments);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Comments(ThreadReqDto dto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Json(_threadService.CreateThread(dto, userId));
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
        [HttpPost]
        [Authorize]
        public IActionResult OnVote(int id, bool isUp)
        {
            if (_threadService.GetThreadDetails(id) == null) throw new ArgumentNullException("Error");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vote = _voteService.GetByThreadIdAndUserId(id, userId);
            if (vote == null)
            {
                vote = new()
                {
                    IsUp = isUp,
                    ThreadId = id,
                    UserId = userId
                };
                _voteService.Create(vote);
            }
            else if (isUp == vote.IsUp) _voteService.RemoveByThreadIdAndUserId(id, userId);
            else _voteService.UpdateByThreadIdAndUserId(id, userId, isUp);


            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
