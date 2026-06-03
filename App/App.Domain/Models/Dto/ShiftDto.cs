using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Domain.Models.Dto
{
    public sealed class ShiftDto : BaseDto<TblShift>
    {
        public int Id { get; set; }
        public string? ShiftCode { get; set; }
         
        public string? ShiftName { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public TimeOnly? BreakStart { get; set; }

        public TimeOnly? BreakEnd { get; set; }
         
        public decimal? WorkingHours { get; set; }

        public bool? IsOvernight { get; set; }

        public bool? IsActive { get; set; }
    }
}
