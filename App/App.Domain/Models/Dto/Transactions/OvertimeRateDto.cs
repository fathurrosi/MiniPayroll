using System.ComponentModel.DataAnnotations;
using App.Domain.Entities;

namespace App.Domain.Models.Dto.Transactions
{
    public class OvertimeRateDto : BaseDto<TblOvertimeRate>
    {
        public int Id { get; set; }

        [Display(Name = "Hour From")]
        public int? HourFrom { get; set; }

        [Display(Name = "Hour To")]
        public int? HourTo { get; set; }

        [Display(Name = "Multiplier")]
        public decimal? Multiplier { get; set; }

        [Display(Name = "Effective Date")]
        public DateOnly? EffectiveDate { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }
    }
}
