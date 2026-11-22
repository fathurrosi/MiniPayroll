using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Settings
{
    public interface IBpjsKesehatanService
    {
        Task<PagedResponse<BpjsKesehatanDto>> GetPagedAsync(DataTableRequest model);
        Task<List<BpjsKesehatanDto>> GetListAsync();
        Task<BpjsKesehatanDto> GetByIdAsync(int id);
        Task<BpjsKesehatanDto> SaveAsync(BpjsKesehatanDto dto);
        Task<int> DeleteAsync(int id);
    }
}
