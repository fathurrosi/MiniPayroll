using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Domain.Models.Dto.Payroll
{ 
    public class PayslipDto //: BaseDto<TblCompanyPayslip>
    {
        public int CompanyPayslipId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CompanyAddress { get; set; }

        public string? LogoFileName { get; set; }
        public string? LogoFilePath { get; set; }

        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? TaxNumber { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
