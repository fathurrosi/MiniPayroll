using App.Domain.Entities; 
using System.ComponentModel.DataAnnotations;
 

namespace App.Domain.Models.Dto.Masters
{
    public sealed class OvertimeTypeDto : BaseDto<TblOvertimeType>
    { 

        [MaxLength(20)] 
        public string? OvertimeCode { get; set; }

        [MaxLength(100)] 
        public string? OvertimeName { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public bool? IsWorkingDay { get; set; }

        public bool? IsRestDay { get; set; }

        public bool? IsPublicHoliday { get; set; }

        public bool? IsActive { get; set; }

        //public string OvertimeCategory { get; set; }
        //private string _OvertimeCategory;

        //public string OvertimeCategory
        //{
        //    get {
        //        if (this.IsWorkingDay == true)
        //            _OvertimeCategory = OvertimeCategory.WorkingDay.ToString();
        //        else if (this.IsRestDay == true)
        //            _OvertimeCategory = OvertimeCategory.RestDay.ToString();
        //        else if (this.IsPublicHoliday == true)
        //            _OvertimeCategory = OvertimeCategory.PublicHoliday.ToString();

        //        return _OvertimeCategory; }
        //    set { _OvertimeCategory = value; }
        //}
        public string OvertimeCategory
        {
            get => IsWorkingDay == true ? nameof(App.Domain.Enums.OvertimeCategory.WorkingDay)
                 : IsRestDay == true ? nameof(App.Domain.Enums.OvertimeCategory.RestDay)
                 : IsPublicHoliday == true ? nameof(App.Domain.Enums.OvertimeCategory.PublicHoliday)
                 : string.Empty;

            set
            {
                IsWorkingDay = value == nameof(App.Domain.Enums.OvertimeCategory.WorkingDay);
                IsRestDay = value == nameof(App.Domain.Enums.OvertimeCategory.RestDay);
                IsPublicHoliday = value == nameof(App.Domain.Enums.OvertimeCategory.PublicHoliday);
            }
        }

    }
}
