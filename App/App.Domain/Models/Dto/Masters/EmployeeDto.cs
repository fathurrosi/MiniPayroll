using App.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Models.Dto.Masters
{
    public sealed class EmployeeDto : BaseDto<TblEmployee>
    {
        [Display(Name = "Employee ID")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee Code is required.")]
        [StringLength(20, ErrorMessage = "Employee Code cannot exceed 20 characters.")]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(255, ErrorMessage = "Full Name cannot exceed 255 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Resign Date")]
        public DateTime? ResignDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Department")]
        public string? Department { get; set; }

        [StringLength(50)]
        [Display(Name = "Position")]
        public string? Position { get; set; }

        [Required(ErrorMessage = "Basic Salary is required.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue,
            ErrorMessage = "Basic Salary must be greater than or equal to 0.")]
        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255)]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(50)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [StringLength(1000)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [StringLength(100)]
        [Display(Name = "Bank Name")]
        public string? BankName { get; set; }

        [StringLength(50)]
        [Display(Name = "Bank Account Number")]
        public string? BankAccountNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "NPWP")]
        public string? Npwp { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }

        [StringLength(50)]
        [Display(Name = "Updated By")]
        public string? UpdatedBy { get; set; }

        [Display(Name = "Default Shift")]
        public int? DefaultShift { get; set; }

        [StringLength(50)]
        [Display(Name = "PTKP Code")]
        public string? Ptkpcode { get; set; }

    }
}
