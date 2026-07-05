using App.Domain.Models.Dto.Attendance;
using App.Domain.Models.Request;
using App.Domain.Models.Response; 
namespace App.Application.Interfaces.Services.Attendance
{
    public interface IAttendanceService
    {
        Task<PagedResponse<VwAttendanceDto>> GetPagedAsync(DataTableRequest model);
        Task<List<AttendanceDto>> GetListAsync();
        Task<AttendanceDto> SaveAsync(AttendanceDto AttendanceDto);
        Task<int> DeleteAsync(long Id);
        Task<AttendanceDto> GetByKeyAsync(int Id);
    }
}
