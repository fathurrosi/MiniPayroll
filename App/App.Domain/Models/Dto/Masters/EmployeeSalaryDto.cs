using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Domain.Models.Dto.Masters
{
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

        public Guid EmployeeId { get; set; }

        public DateOnly EffectiveDate { get; set; }

        public bool IsActive { get; set; }

    }
}
