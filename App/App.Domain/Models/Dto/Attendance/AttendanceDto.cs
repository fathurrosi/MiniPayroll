using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Attendance
{
    public sealed class VwAttendanceDto : BaseDto<VwAttendance>
    {
        public int EmployeeId { get; set; }

        [MaxLength(20)]
        
        public string? EmployeeCode { get; set; }

        [MaxLength(255)]
        
        public string? EmployeeName { get; set; }

        public DateOnly AttendanceDate { get; set; }

        public int? ShiftId { get; set; }

        [MaxLength(20)]
        
        public string? ShiftCode { get; set; }

        [MaxLength(100)]
        
        public string? ShiftName { get; set; }


        public TimeOnly? ScheduledTimeIn { get; set; }


        public TimeOnly? ScheduledTimeOut { get; set; }


        public TimeOnly? ActualTimeIn { get; set; }


        public TimeOnly? ActualTimeOut { get; set; }

        public int LateMinutes { get; set; }

        public int EarlyOutMinutes { get; set; }

        public int WorkMinutes { get; set; }

        public int OvertimeMinutes { get; set; }

        public string AttendanceStatus { get; set; } = null!;

        public string? Remarks { get; set; }

        public bool IsHoliday { get; set; }

        public bool IsWeekend { get; set; }

        public bool IsManualEntry { get; set; }

        [MaxLength(50)]
        
        public string Position { get; set; } = null!;

        [MaxLength(50)]
        
        public string Department { get; set; } = null!;

        [MaxLength(100)]
        
        public string PositionDescription { get; set; } = null!;

        [MaxLength(100)]
        
        public string DepartmentDescription { get; set; } = null!;
    }
    public sealed class AttendanceDto : BaseDto<TblAttendance>
    {
        [Display(Name = "Attendance ID")]
        public int AttendanceId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Attendance Date")]
        public DateOnly AttendanceDate { get; set; }

        [Display(Name = "Shift")]
        public int? ShiftId { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Scheduled Time In")]
        public TimeOnly? ScheduledTimeIn { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Scheduled Time Out")]
        public TimeOnly? ScheduledTimeOut { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Actual Time In")]
        public TimeOnly? ActualTimeIn { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Actual Time Out")]
        public TimeOnly? ActualTimeOut { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Late Minutes")]
        public int LateMinutes { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Early Out Minutes")]
        public int EarlyOutMinutes { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Work Minutes")]
        public int WorkMinutes { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Overtime Minutes")]
        public int OvertimeMinutes { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Attendance Status")]
        public string AttendanceStatus { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "Remarks")]
        public string? Remarks { get; set; }

        [Display(Name = "Holiday")]
        public bool IsHoliday { get; set; }

        [Display(Name = "Weekend")]
        public bool IsWeekend { get; set; }

        [Display(Name = "Manual Entry")]
        public bool IsManualEntry { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [MaxLength(100)]
        [Display(Name = "Modified By")]
        public string? ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // Navigation Properties (optional in DTO)
        public TblEmployee? Employee { get; set; }

        public TblShift? Shift { get; set; }
    }

}
