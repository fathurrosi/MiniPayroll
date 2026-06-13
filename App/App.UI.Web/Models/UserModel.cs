using App.Domain.Models;
using App.Domain.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace App.UI.Web.Models
{
    public sealed class UserModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }

    public class ShiftScheduleModel : PageModel<ShiftDto>
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public string DepartmentCode { get; set; }
        public int? EmployeeId { get; set; }


        public DateTime ModalDateFrom { get; set; }
        public DateTime ModalDateTo { get; set; }

        public string ModalDepartmentCode { get; set; }
        public int? ModalEmployeeId { get; set; }

        public int ModalPatternId { get; set; }
        public bool ModalOverwriteExisting { get; set; }
        public List<EmployeeDto> Employees { get; set; } = new();
        public List<ShiftDto> Shifts { get; set; } = new();
        public List<ShiftPatternDto> ShiftPatterns { get; set; } = new();

        public List<DepartmentDto> Departments { get; set; } = new();
    }

}
