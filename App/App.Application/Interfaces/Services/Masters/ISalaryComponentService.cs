
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Payroll
{

    public interface ISalaryComponentService
    {
        Task<List<SalaryComponentDto>> GetListAsync();
        Task<SalaryComponentDto?> GetByCodeAsync(string componentCode);
        Task<int> DeleteAsync(string code);
        Task<PagedResponse<SalaryComponentDto>> GetPagedAsync(DataTableRequest model);
        Task<SalaryComponentDto> SaveAsync(SalaryComponentDto model);
    }
}
