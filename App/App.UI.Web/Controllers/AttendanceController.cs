using App.Application.Interfaces.Services;
using App.Domain.Models;
using App.Domain.Models.Dto;
using App.Domain.Models.Dto.Masters;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class AttendanceController : BaseController
    {
        private readonly ILogger<AttendanceController> _logger;
        private readonly IUserService _userService;
        public AttendanceController(ILogger<AttendanceController> logger
            , IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        // GET: AttendanceController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Configuration()
        {
            var user = await _userService.GetUserAsync();
            var model = new PageModel<List<MenuDto>>() { Title = "Attendance Configuration" };
            model.Item = new List<MenuDto>();

            if (user != null)
                model.Item = user.ConfigMenuList.Where( t=> t.ParentId == "ATT004").ToList();
            return View(model);
        }

        // GET: AttendanceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AttendanceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttendanceController/Create
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

        // GET: AttendanceController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttendanceController/Edit/5
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

        // GET: AttendanceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttendanceController/Delete/5
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
