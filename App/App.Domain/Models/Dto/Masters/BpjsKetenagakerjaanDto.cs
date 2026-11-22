using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Masters
{
    public sealed class BpjsKetenagakerjaanDto : BaseDto<TblBpjsketenagakerjaanConfig>
    {
        public int Id { get; set; }

        [Display(Name = "JHT Employee Rate (%)")]
        public decimal? JhtEmployeeRate { get; set; }

        [Display(Name = "JHT Company Rate (%)")]
        public decimal? JhtCompanyRate { get; set; }

        [Display(Name = "JP Employee Rate (%)")]
        public decimal? JpEmployeeRate { get; set; }

        [Display(Name = "JP Company Rate (%)")]
        public decimal? JpCompanyRate { get; set; }

        [Display(Name = "JKK Company Rate (%)")]
        public decimal? JkkCompanyRate { get; set; }

        [Display(Name = "JKM Company Rate (%)")]
        public decimal? JkmCompanyRate { get; set; }

        [Display(Name = "Salary Cap")]
        public decimal? SalaryCap { get; set; }

        [Required(ErrorMessage = "Effective Date is required.")]
        [Display(Name = "Effective Date")]
        public DateOnly EffectiveDate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}
