using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace App.Domain.Models.Dto.Masters
{
    public sealed class BranchDto : BaseDto<BranchDto>
    {
        [Required(ErrorMessage = "Branch code is required.")]
        [StringLength(20, ErrorMessage = "Branch code cannot exceed 20 characters.")]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "Branch code must be uppercase only and contain no spaces.")]
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Required(ErrorMessage = "Branch Name is required.")]
        [StringLength(100, ErrorMessage = "Branch Name cannot exceed 100 characters.")]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(191, ErrorMessage = "Address cannot exceed 191 characters.")]
        public string Address { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = false;

        public int BranchHeadEmployeeId { get; set; }
        public int CompanyProfileId { get; set; }
    }
}
