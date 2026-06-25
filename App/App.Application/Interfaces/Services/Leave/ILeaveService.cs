using App.Domain.Models.Dto.Leave;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Leave
{
    public interface ILeaveService
    {
        Task<PagedResponse<LeaveDto>> GetPagedAsync(DataTableRequest model);
        Task<List<LeaveDto>> GetListAsync();
        Task<LeaveDto> SaveAsync(LeaveDto leaveDto);
        Task<int> DeleteAsync(long Id);
        Task<LeaveDto> GetByCodeAsync(int Id);
    }
}
