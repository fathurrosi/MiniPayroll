using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Domain.Models.Dto
{
    public partial class ShiftPatternDto : BaseDto<TblShiftPattern>
    {  
        public int Id { get; set; }
        public string? PatternCode { get; set; } 
        public string? PatternName { get; set; }
        public int? CycleLength { get; set; }
    }
}
