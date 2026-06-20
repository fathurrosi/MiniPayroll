using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Web.Models
{
    public class EmployeeSalaryModel : PageModel<EmployeeSalaryDto>
    {
        public List<EmployeeSalaryDetailDto> Details { get; set; }
    }
}
