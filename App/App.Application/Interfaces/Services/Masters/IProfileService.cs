using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
 
namespace App.Application.Interfaces.Services.Masters
{
    public interface IProfileService
    {
        Task<List<ProfileDto>> GetAllAsync();

        Task<ProfileDto?> GetByIdAsync(int companyProfileId);

        Task<int> CreateAsync(ProfileDto dto);

        Task<bool> UpdateAsync(ProfileDto dto);

        Task<int> DeleteAsync(int companyProfileId);

        Task<PagedResponse<ProfileDto>> GetPagedAsync(DataTableRequest model);
        //Task<List<ProfileDto>> GetCountriesAsync();
        Task<ProfileDto> Save(ProfileDto model);  
    }
}
