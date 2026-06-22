using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace App.Domain.Models.Dto.Masters
{
    public sealed class PositionDto : BaseDto<TblPosition>
    {
        [Required(ErrorMessage = "Position code is required.")]
        [StringLength(50, ErrorMessage = "Position code cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "Position code must be uppercase only and contain no spaces.")]
        [Display(Name = "Position Code")]
        public string PositionCode { get; set; }

        [Required(ErrorMessage = "Position Name is required.")]
        [StringLength(100, ErrorMessage = "Position Name cannot exceed 100 characters.")]
        [Display(Name = "Position Name")]
        public string? PositionName { get; set; }

        [Display(Name = "Grade Level")]
        public string? GradeLevel { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = false;
    }
}
