using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
namespace App.Application.Interfaces.Services.Masters
{
    public interface IEmployeeService
    {
        Task<PagedResponse<EmployeeDto>> GetPagedAsync(DataTableRequest model);
        Task<List<EmployeeDto>> GetListAsync();
        Task<EmployeeDto> Save(EmployeeDto model);
        Task<int> Delete(string code);
        Task<EmployeeDto> GetByCode(string code);
    }
}
