using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Models.Request
{
    public sealed class EmployeeSalaryDataTableRequest : DataTableRequest
    {
        //public int Year { get; set; }
        //public int Month { get; set; }
        public string EmployeeId { get; set; }
        public string PositionCode { get; set; }
        public string DepartmentCode { get; set; }
        //public string EmployeeId { get; set; }
    }
    public sealed class ScheduleDataTableRequest : DataTableRequest
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public string DepartmentCode { get; set; }
        public string EmployeeId { get; set; }
    }
}
