using App.Domain.Entities;
using App.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Dto.Payroll
{
    public class SalaryComponentDto : BaseDto<TblSalaryComponent>
    {
        [Required]
        [StringLength(20)]
        public string ComponentCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ComponentName { get; set; } = string.Empty;

        [Required]
        public SalaryComponentType ComponentType { get; set; }

        [Required]
        public CalculationType CalculationType { get; set; }


        //[Required]
        //public string ComponentType { get; set; } = string.Empty;

        //[Required]
        //public string CalculationType { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal? DefaultAmount { get; set; }

        public bool IsTaxable { get; set; } = false;

        public bool IsActive { get; set; } = true;

        [Range(0, 999)]
        public int SortOrder { get; set; } = 0;

        // Display fields for grid
        public string? ComponentTypeText
        {
            get
            {
                return ComponentType.ToString();
            }
        }

        public string? CalculationTypeText
        {
            get
            {
                return CalculationType.ToString();
            }
        }
    }
    //public class SalaryComponentDto : BaseDto<TblSalaryComponent>
    //{
    //    public string ComponentCode { get; set; } = null!;

    //    public string ComponentName { get; set; } = null!;

    //    public string ComponentType { get; set; } = null!;

    //    public string CalculationType { get; set; } = null!;

    //    public decimal? DefaultAmount { get; set; }

    //    public bool? IsTaxable { get; set; }

    //    public bool? IsActive { get; set; }

    //    public int? SortOrder { get; set; }

    //}
}
