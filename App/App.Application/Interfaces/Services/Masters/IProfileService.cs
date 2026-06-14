using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Masters
{
    public interface IProfileService
    {
        Task<List<ProfileDto>> GetListAsync();
        Task<ProfileDto?> GetByIdAsync(int id);  
        Task<int> DeleteAsync(int id);
        Task<PagedResponse<ProfileDto>> GetPagedAsync(DataTableRequest model); 
        Task<ProfileDto> SaveAsync(ProfileDto model);  
    }
}
