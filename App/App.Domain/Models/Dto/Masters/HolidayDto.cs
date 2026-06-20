using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Domain.Models.Dto.Masters
{
    public class HolidayDto : BaseDto<TblHoliday>
    {
        public int Id { get; set; }

        public DateTime HolidayDate { get; set; }
         
        public string HolidayName { get; set; } = null!;

        public bool IsNationalHoliday { get; set; }
    }
}
