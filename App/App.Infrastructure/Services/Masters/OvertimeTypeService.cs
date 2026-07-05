using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Entities;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services.Masters
{
    public class OvertimeTypeService : IOvertimeTypeService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblOvertimeType> _OvertimeTypeRepo;
        private readonly ILogger<OvertimeTypeService> _logger;
        private readonly IContextService _context;
        public OvertimeTypeService(
            ILogger<OvertimeTypeService> logger,
           IGenericRepository<TblOvertimeType> OvertimeTypeRepo,
           IContextService context,
            IMapper mapper)
        {
            _logger = logger;
            _OvertimeTypeRepo = OvertimeTypeRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<OvertimeTypeDto>> GetListAsync()
        {
            var entities = await _OvertimeTypeRepo.GetListAsync();
            return _mapper.Map<List<OvertimeTypeDto>>(entities);
        }

        public async Task<OvertimeTypeDto?> GetByCodeAsync(string code)
        {
            var entity = await _OvertimeTypeRepo.GetFirstOrDefaultAsync(x =>
                    x.OvertimeCode == code);

            return entity == null
                ? null
                : _mapper.Map<OvertimeTypeDto>(entity);
        }

        public async Task<int> DeleteAsync(string code)
        {
            var entity = await _OvertimeTypeRepo.FindAsync(x =>
                x.OvertimeCode == code);
            if (entity == null)
                return 0;

            return await _OvertimeTypeRepo.Remove(entity);
        }

        public async Task<PagedResponse<OvertimeTypeDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _OvertimeTypeRepo.GetPagedAsync(model);                 
                return entityResult.MapPaged<TblOvertimeType, OvertimeTypeDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged OvertimeTypes");
                throw;
            }
        }



        public async Task<OvertimeTypeDto> SaveAsync(OvertimeTypeDto model)
        {
            try
            {
                var existingItem = await _OvertimeTypeRepo.FindAsync(t => t.OvertimeCode == model.OvertimeCode);
                if (existingItem == null)
                {
                    var item = _mapper.Map<TblOvertimeType>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.Now;
                    var addedItem = await _OvertimeTypeRepo.AddAsync(item);
                    return _mapper.Map<OvertimeTypeDto>(addedItem);
                }
                else
                {
                    _mapper.Map(model, existingItem);
                    existingItem.UpdatedBy = _context.Username;
                    existingItem.UpdatedDate = DateTime.Now;
                    var updatedItem = await _OvertimeTypeRepo.UpdateAsync(existingItem);
                    return _mapper.Map<OvertimeTypeDto>(updatedItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving OvertimeType");
                throw;
            }
        }
    }
}
