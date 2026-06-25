using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Leave
{
    public sealed class LeaveDto : BaseDto<TblLeaveRequest>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Branch code is required.")]
        [StringLength(20, ErrorMessage = "Branch code cannot exceed 20 characters.")]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "Branch code must be uppercase only and contain no spaces.")]
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Required(ErrorMessage = "Department code is required.")]
        [StringLength(50, ErrorMessage = "Department code cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "Department code must be uppercase only and contain no spaces.")]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; } = null!;

        [Required(ErrorMessage = "Employee Id is required.")]
        public long EmployeeId { get; set; }

        [Required(ErrorMessage = "Leave code is required.")]
        [StringLength(20, ErrorMessage = "Leave code cannot exceed 20 characters.")]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "Leave code must be uppercase only and contain no spaces.")]
        [Display(Name = "Leave Code")]
        public string LeaveTypeCode { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "Total Days is required.")]
        public int TotalDays { get; set; }

        [Required(ErrorMessage = "Reason is required.")]
        [Display(Name = "Reason")]
        public string Reason { get; set; } = null!;
    }
}
