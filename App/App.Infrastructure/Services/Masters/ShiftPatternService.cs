using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Entities;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services.Masters
{
    public class ShiftPatternService : IShiftPatternService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblShiftPattern> _ShiftPatternRepo;

        private readonly IGenericRepository<TblShiftPatternDetail> _ShiftPatternDetailRepo;
        private readonly ILogger<ShiftPatternService> _logger;
        private readonly IContextService _context;
        public ShiftPatternService(
            ILogger<ShiftPatternService> logger,
           IGenericRepository<TblShiftPattern> ShiftPatternRepo,
           IContextService context,
            IMapper mapper, IGenericRepository<TblShiftPatternDetail> ShiftPatternDetailRepo
            )
        {
            _logger = logger;
            _ShiftPatternRepo = ShiftPatternRepo;
            _mapper = mapper;
            _context = context;
            _ShiftPatternDetailRepo = ShiftPatternDetailRepo;
        }

        public async Task<List<ShiftPatternDto>> GetListAsync()
        {
            var entities = await _ShiftPatternRepo.GetListAsync();
            return _mapper.Map<List<ShiftPatternDto>>(entities);
        }

        public async Task<ShiftPatternDto?> GetByIdAsync(int id)
        {
            var entity = await _ShiftPatternRepo.GetFirstOrDefaultAsync(x =>
                    x.Id == id);

        }
            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _ShiftPatternRepo.FindAsync(x =>
                x.Id == id);
            if (entity == null)
                return 0;

            var entityDetails = await _ShiftPatternDetailRepo.GetListAsync(t => t.PatternId == id);
            if (entityDetails.Any())
                await _ShiftPatternDetailRepo.RemoveRangeAsync(entityDetails);

            return await _ShiftPatternRepo.Remove(entity);
        }

        public async Task<PagedResponse<ShiftPatternDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _ShiftPatternRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblShiftPattern, ShiftPatternDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Employees");
                throw;
            }
        }



        public async Task<ShiftPatternDto> SaveAsync(ShiftPatternDto model)
        {
            try
            {
                ShiftPatternDto result;
                var existingItem = await _ShiftPatternRepo.FindAsync(t => t.Id.Equals(model.Id));
                if (existingItem == null)
                {
                    var item = _mapper.Map<TblShiftPattern>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.Now;
                }
                else
                {
                    _mapper.Map(model, existingItem);
                    existingItem.UpdatedBy = _context.Username;
                    existingItem.UpdatedDate = DateTime.Now;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving ShiftPattern");
                throw;
            }
        }
    }
}
