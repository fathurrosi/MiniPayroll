using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Settings
{
    public sealed class VwUserDto : BaseDto<VwUser>
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public int? EmployeeId { get; set; }

        public string EmployeeCode { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? HireDate { get; set; }

        public DateTime? ResignDate { get; set; }

        public string Department { get; set; }

        public string Position { get; set; }

        public decimal? BasicSalary { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string BankName { get; set; }

        public string BankAccountNumber { get; set; }

        public string Npwp { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public int? DefaultShift { get; set; }

        public string Ptkpcode { get; set; }


        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password do not match.")]
        public string? ConfirmPassword { get; set; }

        public List<MenuDto> MenuList { get; set; }

        public List<MenuDto> ConfigMenuList { get; set; }
    }
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
