using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
namespace App.Application.Interfaces.Services.Masters
{
    public interface IDepartmentService
    {
        Task<PagedResponse<DepartmentDto>> GetPagedAsync(DataTableRequest model);
        Task<List<DepartmentDto>> GetListAsync();
        Task<DepartmentDto> SaveAsync(DepartmentDto model);
        Task<int> DeleteAsync(string code);
        Task<DepartmentDto> GetByCodeAsync(string code);
    }
}
