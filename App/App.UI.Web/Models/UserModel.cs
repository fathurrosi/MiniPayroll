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

    //public sealed class ScheduleModel
    //{
    //    public int Year { get; set; }
    //    public int Month { get; set; }
    //    public string DepartmentCode { get; set; }
    //    public int? EmployeeId { get; set; }


    //    public int PatternId { get; set; }
    //    public bool OverwriteExisting { get; set; }
    //}



    public class ShiftScheduleModel
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public string DepartmentCode { get; set; }
        public int? EmployeeId { get; set; }


        public int ModalYear { get; set; }
        public int ModalMonth { get; set; }

        public string ModalDepartmentCode { get; set; }
        public int? ModalEmployeeId { get; set; }
        //public ScheduleModel Modal { get; set; }

        public int ModalPatternId { get; set; }
        public bool ModalOverwriteExisting { get; set; }
        public List<EmployeeDto> Employees { get; set; } = new();
        public List<ShiftDto> Shifts { get; set; } = new();
        public List<ShiftPatternDto> ShiftPatterns { get; set; } = new();

        public List<DepartmentDto> Departments { get; set; } = new();
    }

}
