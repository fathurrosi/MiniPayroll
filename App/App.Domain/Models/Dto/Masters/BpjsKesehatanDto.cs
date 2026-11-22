using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Masters
{
    public sealed class BpjsKesehatanDto : BaseDto<TblBpjskesehatanConfig>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee Rate is required.")]
        [Display(Name = "Employee Rate (%)")]
        public decimal EmployeeRate { get; set; }

        [Required(ErrorMessage = "Company Rate is required.")]
        [Display(Name = "Company Rate (%)")]
        public decimal CompanyRate { get; set; }

        [Required(ErrorMessage = "Salary Cap is required.")]
        [Display(Name = "Salary Cap")]
        public decimal SalaryCap { get; set; }

        [Required(ErrorMessage = "Effective Date is required.")]
        [Display(Name = "Effective Date")]
        public DateOnly EffectiveDate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}
