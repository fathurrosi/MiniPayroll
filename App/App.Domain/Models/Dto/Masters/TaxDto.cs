using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Masters
{
    /// <summary>
    /// DTO for PPh21 Tax Bracket (TblPph21Bracket)
    /// </summary>
    public sealed class Pph21BracketDto : BaseDto<TblPph21Bracket>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Min Amount is required.")]
        [Display(Name = "Min Amount")]
        public decimal MinAmount { get; set; }

        [Display(Name = "Max Amount")]
        public decimal? MaxAmount { get; set; }

        [Required(ErrorMessage = "Tax Rate is required.")]
        [Display(Name = "Tax Rate (%)")]
        public decimal TaxRate { get; set; }

        [Required(ErrorMessage = "Effective Date is required.")]
        [Display(Name = "Effective Date")]
        public DateOnly EffectiveDate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO for Biaya Jabatan Config (TblBiayaJabatanConfig)
    /// </summary>
    public sealed class BiayaJabatanDto : BaseDto<TblBiayaJabatanConfig>
    {
        public int Id { get; set; }

        [Display(Name = "Rate (%)")]
        public decimal? Rate { get; set; }

        [Display(Name = "Max Monthly")]
        public decimal? MaxMonthly { get; set; }

        [Display(Name = "Max Yearly")]
        public decimal? MaxYearly { get; set; }

        [Required(ErrorMessage = "Effective Date is required.")]
        [Display(Name = "Effective Date")]
        public DateOnly EffectiveDate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}
