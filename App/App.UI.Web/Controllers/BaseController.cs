using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
