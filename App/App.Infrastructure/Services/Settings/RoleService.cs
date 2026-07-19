using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Masters;
using App.Application.Interfaces.Services.Settings;
using App.Domain.Entities;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services.Settings
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblRole> _RoleRepo;
        private readonly ILogger<RoleService> _logger;
        private readonly IContextService _context;
        public RoleService(
            ILogger<RoleService> logger,
           IGenericRepository<TblRole> RoleRepo,
           IContextService context,
            IMapper mapper)
        {
            _logger = logger;
            _RoleRepo = RoleRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<RoleDto>> GetListAsync()
        {
            var entities = await _RoleRepo.GetListAsync();
            return _mapper.Map<List<RoleDto>>(entities);
        }

        public async Task<RoleDto?> GetByCodeAsync(string code)
        {
            var entity = await _RoleRepo.GetFirstOrDefaultAsync(x =>
                    x.RoleCode == code);

            return entity == null
                ? null
                : _mapper.Map<RoleDto>(entity);
        }

        public async Task<RoleDto?> DeleteAsync(string code)
        {
            var entity = await _RoleRepo.FindAsync(x =>
                x.RoleCode == code);
            if (entity == null)
                return null;

            entity.IsDeleted = true;
            entity  = await _RoleRepo.UpdateAsync(entity);

            return _mapper.Map<RoleDto>(entity);
        }

        public async Task<PagedResponse<RoleDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _RoleRepo.GetPagedAsync(model);                 
                return entityResult.MapPaged<TblRole, RoleDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Roles");
                throw;
            }
        }



        public async Task<RoleDto> SaveAsync(RoleDto model)
        {
            try
            {
                var existingItem = await _RoleRepo.FindAsync(t => t.RoleCode == model.RoleCode);
                if (existingItem == null)
                {
                    var item = _mapper.Map<TblRole>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.Now;
                    var addedItem = await _RoleRepo.AddAsync(item);
                    return _mapper.Map<RoleDto>(addedItem);
                }
                else
                {
                    long roleId = existingItem.Id;
                    _mapper.Map(model, existingItem);
                    existingItem.Id = roleId;
                    existingItem.UpdatedBy = _context.Username;
                    existingItem.UpdatedDate = DateTime.Now;
                    var updatedItem = await _RoleRepo.UpdateAsync(existingItem);
                    return _mapper.Map<RoleDto>(updatedItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Role");
                throw;
            }
        }
    }
}
