using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Settings
{
    public interface IBpjsKetenagakerjaanService
    {
        Task<PagedResponse<BpjsKetenagakerjaanDto>> GetPagedAsync(DataTableRequest model);
        Task<List<BpjsKetenagakerjaanDto>> GetListAsync();
        Task<BpjsKetenagakerjaanDto> GetByIdAsync(int id);
        Task<BpjsKetenagakerjaanDto> SaveAsync(BpjsKetenagakerjaanDto dto);
        Task<int> DeleteAsync(int id);
    }
}
