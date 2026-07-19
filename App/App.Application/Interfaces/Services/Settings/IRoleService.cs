using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Settings
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetListAsync();
        Task<RoleDto?> GetByCodeAsync(string code);
        Task<RoleDto> DeleteAsync(string code);
        Task<PagedResponse<RoleDto>> GetPagedAsync(DataTableRequest model);
        Task<RoleDto> SaveAsync(RoleDto model);
    }
}
