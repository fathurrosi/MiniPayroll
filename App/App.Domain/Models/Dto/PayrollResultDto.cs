using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Domain.Models.Dto
{
    public sealed class PayrollResultDto : BaseDto<TblPayrollResult>
    {
        //public int Id { get; set; }

        //public int? EmployeeId { get; set; }

        //public int? PeriodYear { get; set; }

        //public int? PeriodMonth { get; set; }

        //public decimal? GrossSalary { get; set; }

        //public decimal? TotalDeduction { get; set; }

        //public decimal? NetSalary { get; set; }

        //public decimal? Bpjskesehatan { get; set; }

        //public decimal? Bpjsketenagakerjaan { get; set; }

        //public decimal? Pph21 { get; set; }


        public int Id { get; set; }

        public int PayrollRunId { get; set; }

        public int EmployeeId { get; set; }
         
        public decimal? BasicSalary { get; set; }
         
        public decimal? Allowance { get; set; }
         
        public decimal? Overtime { get; set; }
         
        public decimal? GrossSalary { get; set; }
         
        public decimal? Bpjskesehatan { get; set; }
         
        public decimal? Bpjsketenagakerjaan { get; set; }
         
        public decimal? BiayaJabatan { get; set; }
         
        public decimal? Pph21 { get; set; }
         
        public decimal? TotalDeduction { get; set; }
         
        public decimal? NetSalary { get; set; }

 
    }
}
