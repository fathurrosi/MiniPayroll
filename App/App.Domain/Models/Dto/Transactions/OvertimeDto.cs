using System.ComponentModel.DataAnnotations;
using App.Domain.Entities;

namespace App.Domain.Models.Dto.Transactions
{
    public class OvertimeDto : BaseDto<TblOvertime>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee is required.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string? EmployeeName { get; set; }

        [Required(ErrorMessage = "Overtime Date is required.")]
        [Display(Name = "Overtime Date")]
        public DateOnly OvertimeDate { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required.")]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Total Hours")]
        public decimal TotalHours { get; set; }

        [Required(ErrorMessage = "Reason is required.")]
        [Display(Name = "Reason")]
        public string? Reason { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Approved By")]
        public string? ApprovedBy { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }
    }
}
