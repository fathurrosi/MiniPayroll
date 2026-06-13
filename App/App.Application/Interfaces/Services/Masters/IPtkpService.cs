using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
 
namespace App.Application.Interfaces.Services.Masters
{
    public interface IPtkpService
    {
        Task<List<PtkpDto>> GetListAsync();
        Task<PtkpDto?> GetByIdAsync(string code);  
        Task<int> DeleteAsync(string code);
        Task<PagedResponse<PtkpDto>> GetPagedAsync(DataTableRequest model); 
        Task<PtkpDto> SaveAsync(PtkpDto model);  
    }
}
