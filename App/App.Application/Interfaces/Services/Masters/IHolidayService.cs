using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Masters
{
    public interface IPositionService
    {
        Task<List<PositionDto>> GetListAsync();
        Task<PositionDto?> GetByCodeAsync(string code);  
        Task<int> DeleteAsync(string code);
        Task<PagedResponse<PositionDto>> GetPagedAsync(DataTableRequest model); 
        Task<PositionDto> SaveAsync(PositionDto model);  
    }
}
