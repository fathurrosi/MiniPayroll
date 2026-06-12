using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Models
{
    public class BPJSKesehatanConfig
    {
        public decimal EmployeeRate { get; set; }
        public decimal CompanyRate { get; set; }
        public decimal SalaryCap { get; set; }
    }

    public class BPJSKetenagakerjaanConfig
    {
        public decimal JHT_EmployeeRate { get; set; }
        public decimal JHT_CompanyRate { get; set; }

        public decimal JP_EmployeeRate { get; set; }
        public decimal JP_CompanyRate { get; set; }

        public decimal JKK_CompanyRate { get; set; }
        public decimal JKM_CompanyRate { get; set; }
    }

    public class BiayaJabatanConfig
    {
        public decimal Rate { get; set; }
        public decimal MaxMonthly { get; set; }
        public decimal MaxYearly { get; set; }
    }
}
