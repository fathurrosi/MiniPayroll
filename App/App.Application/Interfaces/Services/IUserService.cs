using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services
{
    public interface IUserService
    { 
        Task<bool> ValidateUserAsync(string usenrame, string password);
        Task<PagedResponse<UserDto>> GetPagedAsync(DataTableRequest model);
        Task<List<UserDto>> GetUsersAsync();
        Task<List<UserDto>> GetUsersAsync(string groupName);
        Task<List<UserDto>> GetUsersAsyncJoin();
        Task<UserDto> Save(UserDto model);
        Task<UserDto> SaveUser(UserDto model);
        Task<int> Delete(string name);
        Task<UserDto> GetByKey(string name); 
        Task<UserDto> GetById(int userId);
        Task<List<UserDto>> GetUnMappedusers();
        Task DeleteBulkAsync(List<int> userIds);
        Task<List<UserDto>> GetUsers(List<string> usernames);

        //Task<List<UserDto>> SearchUsers(string searchText);
        //Task<List<UserDto>> GetUserByDisplayName(List<string> displayNames);
        ////Task<List<UserViewDto>> GetUsersViewByGroup(string groupName);
        //Task<List<UserDto>> GetActiveUsersAsync(string searchText = "");
        //Task<List<UserDto>> GetUsersByOriginator(string originator);

        Task<UserDto?> GetUserAsync();
        //Task<UserDto?> GetUserAsync(string username);
    }
}
