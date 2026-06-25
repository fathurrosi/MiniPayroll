using App.Domain.Entities;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Masters
{
    public interface IBranchService
    {
        Task<PagedResponse<BranchDto>> GetPagedAsync(DataTableRequest model);
        Task<List<BranchDto>> GetListAsync();
        Task<BranchDto> SaveAsync(BranchDto branchDto);
        Task<int> DeleteAsync(string code);
        Task<BranchDto> GetByCodeAsync(string code);
    }
}
