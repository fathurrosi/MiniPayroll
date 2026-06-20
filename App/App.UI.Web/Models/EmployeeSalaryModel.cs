using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Web.Models
{
    public class EmployeeSalaryModel : PageModel<List<EmployeeSalaryDto>>
    {
        public List<EmployeeDto> Employees { get; set; }
        public List<SalaryComponentDto> Components { get; set; }
    }
}
