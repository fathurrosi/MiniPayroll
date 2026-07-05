using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Masters
{
    public interface IOvertimeTypeService
    {
        Task<List<OvertimeTypeDto>> GetListAsync();
        Task<OvertimeTypeDto?> GetByCodeAsync(string code);  
        Task<int> DeleteAsync(string code);
        Task<PagedResponse<OvertimeTypeDto>> GetPagedAsync(DataTableRequest model); 
        Task<OvertimeTypeDto> SaveAsync(OvertimeTypeDto model);  
    }
}
