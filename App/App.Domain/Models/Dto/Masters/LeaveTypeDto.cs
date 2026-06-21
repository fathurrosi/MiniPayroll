using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Masters
{
    public sealed class LeaveTypeDto : BaseDto<TblLeaveType>
    {
        [Required(ErrorMessage = "Leave code is required.")]
        [StringLength(20, ErrorMessage = "Leave code cannot exceed 20 characters.")]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "Leave code must be uppercase only and contain no spaces.")]
        public string LeaveCode { get; set; }

        [Required(ErrorMessage = "Leave Name is required.")]
        [StringLength(100, ErrorMessage = "Leave Name cannot exceed 100 characters.")]
        [Display(Name = "Leave Name")]
        public string LeaveName { get; set; }

        [Display(Name = "Default Annual Quota")]
        [Range(1, 365, ErrorMessage = "Total days must be a number between 1 and 365.")]
        public int? DefaultAnnualQuota { get; set; } = 12;

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; } = false;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = false;
    }
}
