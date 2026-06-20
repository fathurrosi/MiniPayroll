using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.UI.Web.Models
{
    public class EmployeeSalaryModel : PageModel<List<EmployeeSalaryDto>>
    {
        public Guid EmployeeSalaryId { get; set; }

        public Guid EmployeeId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public List<EmployeeDto> Employees { get; set; }
        public List<SalaryComponentDto> Components { get; set; }

        public List<DepartmentDto> Departments { get; set; }
        public string DepartmentCode { get; set; }

    }
}
