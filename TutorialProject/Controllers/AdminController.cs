using BusinessLayer;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TutorialProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ThreadDal _threadDal;
        private readonly ThreadService _threadService;
        public AdminController(ThreadDal threadDal, ThreadService threadService)
        {
            _threadDal = threadDal;
            _threadService = threadService;

        }

        // GET: AdminController
        public ActionResult Index()
        {
            var threads = _threadDal.GetListByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(threads);
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return RedirectToPage("Thread", id);
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var thread = _threadDal.Get(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (thread == null || thread.AppUser.Id != userId) throw new BadHttpRequestException("no");
            _threadDal.Delete(thread);
            return Json("ok");
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
