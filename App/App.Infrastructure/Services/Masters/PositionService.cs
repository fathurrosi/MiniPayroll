using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Entities;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services.Masters
{
    public class PositionService : IPositionService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblPosition> _PositionRepo;
        private readonly ILogger<PositionService> _logger;
        private readonly IContextService _context;
        public PositionService(
            ILogger<PositionService> logger,
           IGenericRepository<TblPosition> PositionRepo,
           IContextService context,
            IMapper mapper)
        {
            _logger = logger;
            _PositionRepo = PositionRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<PositionDto>> GetListAsync()
        {
            var entities = await _PositionRepo.GetListAsync();
            return _mapper.Map<List<PositionDto>>(entities);
        }

        public async Task<PositionDto?> GetByCodeAsync(string code)
        {
            var entity = await _PositionRepo.GetFirstOrDefaultAsync(x =>
                    x.PositionCode == code);

            return entity == null
                ? null
                : _mapper.Map<PositionDto>(entity);
        }

        public async Task<int> DeleteAsync(string code)
        {
            var entity = await _PositionRepo.FindAsync(x =>
                x.PositionCode == code);
            if (entity == null)
                return 0;

            return await _PositionRepo.Remove(entity);
        }

        public async Task<PagedResponse<PositionDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _PositionRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblPosition, PositionDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Employees");
                throw;
            }
        }

        public async Task<PositionDto> SaveAsync(PositionDto model)
        {
            try
            {
                var existingItem = await _PositionRepo.FindAsync(t => t.PositionCode == model.PositionCode);
                if (existingItem == null)
                {
                    var item = _mapper.Map<TblPosition>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.Now;
                    var addedItem = await _PositionRepo.AddAsync(item);
                    return _mapper.Map<PositionDto>(addedItem);
                }
                else
                {
                    _mapper.Map(model, existingItem);
                    existingItem.UpdatedBy = _context.Username;
                    existingItem.UpdatedDate = DateTime.Now;
                    var updatedItem = await _PositionRepo.UpdateAsync(existingItem);
                    return _mapper.Map<PositionDto>(updatedItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Position");
                throw;
            }
        }
    }
}
