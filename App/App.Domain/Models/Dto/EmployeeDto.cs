using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Models.Dto
{
    public sealed class EmployeeDto : BaseDto<TblEmployee>
    {
        [Display(Name = "Employee ID")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee code is required.")]
        [StringLength(20, ErrorMessage = "Employee code cannot exceed 20 characters.")]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; } = null!;

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(255, ErrorMessage = "Full name cannot exceed 255 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;

        [StringLength(20, ErrorMessage = "Gender cannot exceed 20 characters.")]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateOnly? BirthDate { get; set; }

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateOnly? HireDate { get; set; }

        [Display(Name = "Resign Date")]
        [DataType(DataType.Date)]
        public DateOnly? ResignDate { get; set; }

        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters.")]
        [Display(Name = "Department")]
        public string? Department { get; set; }

        [StringLength(100, ErrorMessage = "Position cannot exceed 100 characters.")]
        [Display(Name = "Position")]
        public string? Position { get; set; }

        [Required(ErrorMessage = "Basic salary is required.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999999999999999.99,
            ErrorMessage = "Basic salary must be greater than or equal to 0.")]
        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(50, ErrorMessage = "Phone number cannot exceed 50 characters.")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [StringLength(1000, ErrorMessage = "Address cannot exceed 1000 characters.")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "Bank name cannot exceed 100 characters.")]
        [Display(Name = "Bank Name")]
        public string? BankName { get; set; }

        [StringLength(50, ErrorMessage = "Bank account number cannot exceed 50 characters.")]
        [Display(Name = "Bank Account Number")]
        public string? BankAccountNumber { get; set; }

        [StringLength(50, ErrorMessage = "NPWP cannot exceed 50 characters.")]
        [Display(Name = "NPWP")]
        public string? Npwp { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; } = true;
         
    }
}
