using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
 
namespace App.Application.Interfaces.Services.Masters
{
    public interface IHolidayService
    {
        Task<List<HolidayDto>> GetListAsync();
        Task<HolidayDto?> GetByIdAsync(int id);  
        Task<int> DeleteAsync(int id);
        Task<PagedResponse<HolidayDto>> GetPagedAsync(DataTableRequest model); 
        Task<HolidayDto> SaveAsync(HolidayDto model);  
    }
}
