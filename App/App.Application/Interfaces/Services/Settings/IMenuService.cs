using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;

namespace App.Application.Interfaces.Services.Settings
{
    public interface IMenuService
    {
        Task<PagedResponse<MenuDto>> GetPagedAsync(DataTableRequest model);
        Task<List<MenuDto>> GetListAsync();
        Task<MenuDto> Save(MenuDto model);
        Task<int> Delete(string code);
        Task<MenuDto> GetByCode(string code);
    }
}
