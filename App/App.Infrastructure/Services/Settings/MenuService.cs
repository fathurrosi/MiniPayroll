using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Domain.Entities;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using App.Infrastructure.Services.Masters;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace App.Infrastructure.Services.Settings
{
    public sealed class MenuService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblMenu> _menuRepo;
        private readonly ILogger<MenuService> _logger;
        private readonly IContextService _userService;
        public MenuService(IGenericRepository<TblMenu> menuRepo
            , IMapper mapper
            , ILogger<MenuService> logger
            , IContextService userService)
        {
            _menuRepo = menuRepo;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }

        public async Task<int> Delete(string code)
        {
            try
            {
                var entityItem = await _menuRepo.FindAsync(t => t.Id.Equals(code))
                      ?? throw new KeyNotFoundException($"Code {code} not found");
                return await _menuRepo.Remove(entityItem);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, $"Delete blocked by FK for Code : {code}");

                throw new Exception(
                    $"Oops, this record ({code}) is used by another configuration and cannot be deleted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Menu with code {code}", code);
                throw;
            }
        }

        public async Task<MenuDto> GetByCode(string code)
        {
            try
            {
                var entityItem = await _menuRepo.FindAsync(t => t.Id == code);
                return _mapper.Map<MenuDto>(entityItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Menu with code {code}", code);
                throw;
            }
        }

        public async Task<PagedResponse<MenuDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _menuRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblMenu, MenuDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Menus");
                throw;
            }
        }

        public async Task<List<MenuDto>> GetListAsync()
        {
            try
            {
                var entityResult = await _menuRepo.GetListAsync();
                return entityResult.Select(t => _mapper.Map<MenuDto>(t)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Menus");
                throw;
            }
        }

        public async Task<MenuDto> Save(MenuDto model)
        {
            try
            {
                var entityItem = await _menuRepo.FindAsync(t => t.Id.Equals(model.Id));
                if (entityItem == null)
                {
                    TblMenu item = _mapper.Map<TblMenu>(model);
                    var addedEntity = await _menuRepo.AddAsync(item);
                    return _mapper.Map<MenuDto>(addedEntity);
                }
                else
                {
                    _mapper.Map(model, entityItem);
                    var updatedEntity = await _menuRepo.UpdateAsync(entityItem);
                    return _mapper.Map<MenuDto>(updatedEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Menu");
                throw;
            }
        }

    }
}
