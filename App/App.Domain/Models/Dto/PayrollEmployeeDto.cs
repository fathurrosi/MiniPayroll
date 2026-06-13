using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Models.Dto
{
    public sealed class PayrollEmployeeDto
    {
        public int EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Overtime { get; set; }

        public decimal PTKP { get; set; }
        public decimal AttendanceDeduction { get; set; } = 0;
    }
}
