using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Domain.Models.Dto.Masters
{
    public sealed class EmployeeMonthlyScheduleDto : BaseDto<VwEmployeeMonthlySchedule>
    {

        public long? RowNumber { get; set; }
        //public int? EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public int? YearNo { get; set; }
        public int? MonthNo { get; set; }
        public string? Day1 { get; set; }
        public string? Day2 { get; set; }
        public string? Day3 { get; set; }
        public string? Day4 { get; set; }
        public string? Day5 { get; set; }
        public string? Day6 { get; set; }
        public string? Day7 { get; set; }
        public string? Day8 { get; set; }
        public string? Day9 { get; set; }
        public string? Day10 { get; set; }
        public string? Day11 { get; set; }
        public string? Day12 { get; set; }
        public string? Day13 { get; set; }
        public string? Day14 { get; set; }
        public string? Day15 { get; set; }
        public string? Day16 { get; set; }
        public string? Day17 { get; set; }
        public string? Day18 { get; set; }
        public string? Day19 { get; set; }
        public string? Day20 { get; set; }
        public string? Day21 { get; set; }
        public string? Day22 { get; set; }
        public string? Day23 { get; set; }
        public string? Day24 { get; set; }
        public string? Day25 { get; set; }
        public string? Day26 { get; set; }
        public string? Day27 { get; set; }
        public string? Day28 { get; set; }
        public string? Day29 { get; set; }
        public string? Day30 { get; set; }
        public string? Day31 { get; set; }
    }
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

        public bool IsOvernight { get; set; }

        public bool IsActive { get; set; }
    }
}
