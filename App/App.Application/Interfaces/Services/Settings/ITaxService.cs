using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Settings
{
    public interface ITaxService
    {
        // PPh21 Bracket
        Task<PagedResponse<Pph21BracketDto>> GetPph21PagedAsync(DataTableRequest model);
        Task<List<Pph21BracketDto>> GetPph21ListAsync();
        Task<Pph21BracketDto> GetPph21ByIdAsync(int id);
        Task<Pph21BracketDto> SavePph21Async(Pph21BracketDto dto);
        Task<int> DeletePph21Async(int id);

        // Biaya Jabatan
        Task<PagedResponse<BiayaJabatanDto>> GetBiayaJabatanPagedAsync(DataTableRequest model);
        Task<List<BiayaJabatanDto>> GetBiayaJabatanListAsync();
        Task<BiayaJabatanDto> GetBiayaJabatanByIdAsync(int id);
        Task<BiayaJabatanDto> SaveBiayaJabatanAsync(BiayaJabatanDto dto);
        Task<int> DeleteBiayaJabatanAsync(int id);
    }
}
