using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Settings
{
    public sealed class UserDto : BaseDto<TblUser>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(255)]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password do not match.")]
        public string? ConfirmPassword { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(255)]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Last Login")]
        public DateTime? LastLoginDate { get; set; }

        public List<MenuDto> MenuList { get; set; }

        public List<MenuDto> ConfigMenuList { get; set; }
    }

    //public sealed class UserDto : BaseDto<TblUser>
    //{
    //    public long Id { get; set; }
    //    public string Username { get; set; } = null!;
    //    public string? Email { get; set; }
    //    public string PasswordHash { get; set; } = null!;
    //    public string? FullName { get; set; }
    //    public bool IsActive { get; set; }
    //    public DateTime? LastLoginDate { get; set; }

    //    public List<MenuDto> MenuList { get; set; }

    //    public List<MenuDto> ConfigMenuList { get; set; }
    //}
}
