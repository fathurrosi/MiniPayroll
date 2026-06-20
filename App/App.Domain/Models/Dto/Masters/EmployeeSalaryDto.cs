
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

        public Guid? EmployeeSalaryId { get; set; }

        public Dictionary<string, decimal> Components { get; set; }
            = new();
    }
    public sealed class EmployeeSalaryDetailDto : BaseDto<TblEmployeeSalaryDetail>
    {
        public Guid EmployeeSalaryDetailId { get; set; }

        public Guid EmployeeSalaryId { get; set; }


        public string ComponentCode { get; set; } = null!;

        public decimal Amount { get; set; }

    }

    public sealed class EmployeeSalaryDto : BaseDto<TblEmployeeSalary>
    {
        public Guid EmployeeSalaryId { get; set; }

        public int EmployeeId { get; set; }

        public DateOnly EffectiveDate { get; set; }

        public bool IsActive { get; set; }

        public List<EmployeeSalaryDetailDto> Details { get; set; } = new List<EmployeeSalaryDetailDto>();

    }
}
