using App.Domain.Models.Dto;
using App.Domain.Models.Dto.Settings;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> GetHashPassword(string password);
        Task<bool> ValidateUserAsync(string usenrame, string password);
        Task<PagedResponse<UserDto>> GetPagedAsync(DataTableRequest model);
        Task<List<UserDto>> GetUsersAsync(); 
        Task<UserDto> Save(UserDto model); 
        Task<int> Delete(string name);
        Task<UserDto> GetByKey(string name); 
        Task<UserDto> GetById(int userId); 
        Task<List<UserDto>> GetUsers(List<string> usernames); 

        Task<UserDto?> GetUserAsync();
        Task<bool> ClearAsync(); 
    }
}
