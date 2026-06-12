using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class WorkflowController : Controller
    {
        // GET: WorkflowController
        public ActionResult Index()
        {
            return View();
        }

        // GET: WorkflowController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WorkflowController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkflowController/Create
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

        // GET: WorkflowController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WorkflowController/Edit/5
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

        // GET: WorkflowController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WorkflowController/Delete/5
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
