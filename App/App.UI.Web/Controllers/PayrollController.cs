using App.Domain.Models;
using App.Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class PayrollController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Payslip(int id)
        {
            var model = new PageModel<EmployeeDto>();
            return View(model);
        }
    }
}
