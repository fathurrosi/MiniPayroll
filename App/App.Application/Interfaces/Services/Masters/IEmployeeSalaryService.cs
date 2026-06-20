
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Dto.Payroll;
using App.Domain.Models.Request;
using App.Domain.Models.Response; 

namespace App.Application.Interfaces.Services.Masters
{

    public interface IEmployeeSalaryService
    {
        Task<List<EmployeeSalaryDto>> GetListAsync();
        Task<EmployeeSalaryDto?> GetByCodeAsync(string componentCode);
        Task<int> DeleteAsync(string code);
        Task<PagedResponse<EmployeeSalaryDto>> GetPagedAsync(DataTableRequest model);
        Task<EmployeeSalaryDto> SaveAsync(EmployeeSalaryDto model);
    }
}
