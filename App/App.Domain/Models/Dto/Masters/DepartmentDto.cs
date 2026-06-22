using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace App.Domain.Models.Dto.Masters
{
    public sealed class DepartmentDto :BaseDto<TblDepartment>
    {
        [Required(ErrorMessage = "Department code is required.")]
        [StringLength(50, ErrorMessage = "Department code cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "Department code must be uppercase only and contain no spaces.")]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; } = null!;

        [Required(ErrorMessage = "Department Name is required.")]
        [StringLength(100, ErrorMessage = "Department Name cannot exceed 100 characters.")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; } = null!;
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = false;
    }
}
