
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Masters
{
    public sealed class VwEmployeeSalaryDto : BaseDto<VwEmployeeSalary>
    {
        public int EmployeeId { get; set; }

        public string EmployeeCode { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;
         
        public string Position { get; set; }
        public string Department { get; set; }
        public string? PositionDescription { get; set; }
 
        public string? DepartmentDescription { get; set; }

        public Dictionary<string, decimal> Components { get; set; }
            = new();
    }
    public sealed class EmployeeSalaryDetailDto : BaseDto<TblEmployeeSalaryDetail>
    {
        public int EmployeeId { get; set; }

        public string ComponentCode { get; set; } = null!;

        public decimal Amount { get; set; }

    }

    public sealed class EmployeeSalaryDto : BaseDto<TblEmployeeSalary>
    { 
        public int EmployeeId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public bool IsActive { get; set; }
         
        public List<EmployeeSalaryDetailDto> Details { get; set; } = new List<EmployeeSalaryDetailDto>();

    }
}
