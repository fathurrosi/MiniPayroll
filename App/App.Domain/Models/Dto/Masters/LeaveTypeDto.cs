using App.Domain.Entities;

namespace App.Domain.Models.Dto.Masters
{
    public sealed class LeaveTypeDto : BaseDto<TblLeaveType>
    {
        public string LeaveCode { get; set; } = null!;
        public string LeaveName { get; set; } = null!;
        public int? DefaultAnnualQuota { get; set; }
        public bool IsPaid { get; set; }
        public bool IsActive { get; set; }
    }
}
