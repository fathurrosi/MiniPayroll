using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Masters
{
    public interface ILeaveTypeService
    {
        Task<PagedResponse<LeaveTypeDto>> GetPagedAsync(DataTableRequest model);
        Task<List<LeaveTypeDto>> GetListAsync();
        Task<LeaveTypeDto> SaveAsync(LeaveTypeDto leaveTypeDto);
        Task<int> DeleteAsync(string code);
        Task<LeaveTypeDto> GetByCodeAsync(string code);
    }
}
