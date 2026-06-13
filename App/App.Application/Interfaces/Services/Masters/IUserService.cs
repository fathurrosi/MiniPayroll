using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Services.Masters
{
    public interface IUserService
    { 
        Task<PagedResponse<UserDto>> GetPagedAsync(DataTableRequest model);
        Task<List<UserDto>> GetListAsync(); 
        Task<UserDto> Save(UserDto model);
        Task<UserDto> SaveUser(UserDto model);
        Task<int> Delete(string code);
        Task<UserDto> GetByKey(string code);  
        Task<UserDto?> GetUserAsync(); 
    }
}
