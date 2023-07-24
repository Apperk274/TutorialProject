using BusinessLayer;
using DataAccessLayer.Repositories;
using DTOLayer.ReqDTO;
using DTOLayer.ResDTO;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
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
        private readonly SignInManager<AppUser> _signInManager;

        public ThreadController(ILogger<ThreadController> logger, ThreadDal threadDal, ThreadService threadService, VoteService voteService, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _threadDal = threadDal;
            _threadService = threadService;
            _voteService = voteService;
            _signInManager = signInManager;
        }

        [HttpGet]
        public JsonResult Comments(int id)
        {
            string userId = null;
            if (_signInManager.IsSignedIn(User)) userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var comments = _threadDal.GetCommentsOfThread(id);
            var commentsVoteDict = _voteService.GetUpvotesAndDownvotesForThreads(comments.Select(e => e.Id).ToList());
            var res = new ThreadResDto[comments.Count];
            for (var i = 0; i < comments.Count; i++)
            {
                var comment = comments[i];
                var votes = (upVotes: 0, downVotes: 0);
                commentsVoteDict.TryGetValue(comment.Id, out votes);
                Vote myVote = null;
                if (userId != null) myVote = _voteService.GetByThreadIdAndUserId(comment.Id, userId);
                bool? isLiked = myVote?.IsUp;
                var commentResDto = new ThreadResDto
                {
                    Thread = comment,
                    NumOfComments = 0,
                    DownVotes = votes.downVotes,
                    UpVotes = votes.upVotes,
                    IsLiked = isLiked,
                };
                res[i] = commentResDto;
            }

            return Json(res);
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
            threadDetails.DownVotes = DownVotes;
            threadDetails.UpVotes = UpVotes;
            string userId = null;
            if (_signInManager.IsSignedIn(User)) userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Vote myVote = null;
            if (userId != null) myVote = _voteService.GetByThreadIdAndUserId(id, userId);
            bool? isLiked = myVote?.IsUp;
            threadDetails.IsLiked = isLiked;
            return View(threadDetails);
        }
        [HttpPost]
        [Authorize]
        public JsonResult Vote(int id, bool isUp)
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


            return Json(true);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
