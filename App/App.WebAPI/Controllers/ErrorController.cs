using Microsoft.AspNetCore.Mvc;

namespace POS.Api.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
