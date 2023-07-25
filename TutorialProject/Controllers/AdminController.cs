using BusinessLayer;
using DataAccessLayer.Repositories;
using DTOLayer.ReqDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TutorialProject.Models;

namespace TutorialProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ThreadDal _threadDal;
        private readonly CategoryDal _categoryDal;
        private readonly ThreadService _threadService;
        public AdminController(ThreadDal threadDal, ThreadService threadService, CategoryDal categoryDal)
        {
            _threadDal = threadDal;
            _threadService = threadService;
            _categoryDal = categoryDal;
        }
        // GET: AdminController/List
        public JsonResult List()
        {
            var threads = _threadDal.GetListByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return new JsonResult(threads);
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
            var thread = _threadDal.Get(id);
            var categories = _categoryDal.GetAll();
            EditThreadViewModel vm = new()
            {
                Thread = thread,
                AvailableCategories = categories
            };
            return View(vm);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ThreadReqDto dto)
        {
            try
            {
                var thread = _threadDal.Get(id) ?? throw new BadHttpRequestException("no");
                thread.Title = dto.Title;
                thread.Content = dto.Content;
                thread.CategoryId = dto.CategoryId;
                _threadDal.Update(thread);
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


    }
}
