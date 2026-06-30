using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Leave
{
    public sealed class LeaveDto : BaseDto<VwLeaveRequest>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Branch is required.")]
        [Display(Name = "Branch")]
        public string BranchCode { get; set; } = null!;

        [Display(Name = "Branch Name")]
        public string? BranchName { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public string DepartmentCode { get; set; } = null!;

        [Display(Name = "Department Name")]
        public string? DepartmentName { get; set; }

        [Required(ErrorMessage = "Employee is required.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }

        [Required(ErrorMessage = "Leave Type is required.")]
        [Display(Name = "Leave Type")]
        public string LeaveTypeCode { get; set; } = null!;

        [Display(Name = "Leave Type Name")]
        public string? LeaveName { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "Total Days is required.")]
        [Display(Name = "Total Days")]
        public int TotalDays { get; set; }

        [Required(ErrorMessage = "Reason is required.")]
        [Display(Name = "Reason")]
        public string? Reason { get; set; }

        [Display(Name = "Status")]
        public bool? ApprovalStatus { get; set; }

        [Display(Name = "Approved By")]
        public int? ApprovedBy { get; set; }

        [Display(Name = "Approved By Name")]
        public string? ApprovedByName { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }
    }
}
