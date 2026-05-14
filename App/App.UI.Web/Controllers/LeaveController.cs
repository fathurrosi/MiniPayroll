using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class LeaveController : Controller
    {
        // GET: LeaveController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LeaveController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LeaveController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveController/Create
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

        // GET: LeaveController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveController/Edit/5
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

        // GET: LeaveController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveController/Delete/5
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
