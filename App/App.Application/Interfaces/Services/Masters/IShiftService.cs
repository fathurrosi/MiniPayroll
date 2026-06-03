using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
 
namespace App.Application.Interfaces.Services.Masters
{
    public interface IShiftService
    {
        Task GenerateAsync(GenerateScheduleRequest request);
        Task<List<ShiftDto>> GetListAsync();
        Task<ShiftDto?> GetByIdAsync(int id);  
        Task<int> DeleteAsync(int id);
        Task<PagedResponse<ShiftDto>> GetPagedAsync(DataTableRequest model); 
        Task<ShiftDto> SaveAsync(ShiftDto model);  
    }
}
