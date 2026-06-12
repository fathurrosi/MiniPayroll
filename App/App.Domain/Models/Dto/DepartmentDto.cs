using App.Domain.Entities; 
namespace App.Domain.Models.Dto
{
    public sealed class DepartmentDto :BaseDto<TblDepartment>
    { 
        public string DepartmentCode { get; set; } = null!;
         
        public string DepartmentName { get; set; } = null!;

    }
}
