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
    public class PtkpService : IPtkpService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblPtkp> _PtkpRepo;
        private readonly ILogger<PtkpService> _logger;
        private readonly IContextService _context;
        public PtkpService(
            ILogger<PtkpService> logger,
           IGenericRepository<TblPtkp> PtkpRepo,
           IContextService context,
            IMapper mapper)
        {
            _logger = logger;
            _PtkpRepo = PtkpRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<PtkpDto>> GetListAsync()
        {
            var entities = await _PtkpRepo.GetListAsync();
            return _mapper.Map<List<PtkpDto>>(entities);
        }

        public async Task<PtkpDto?> GetByIdAsync(string code)
        {
            var entity = await _PtkpRepo.GetFirstOrDefaultAsync(x =>
                    x.Ptkpcode == code);

            return entity == null
                ? null
                : _mapper.Map<PtkpDto>(entity);
        }

        public async Task<int> DeleteAsync(string code)
        {
            var entity = await _PtkpRepo.FindAsync(x =>
                x.Ptkpcode == code);
            if (entity == null)
                return 0;

            return await _PtkpRepo.Remove(entity);
        }

        public async Task<PagedResponse<PtkpDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _PtkpRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblPtkp, PtkpDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Employees");
                throw;
            }
        }

        public async Task<PtkpDto> SaveAsync(PtkpDto model)
        {
            try
            {
                var existingItem = await _PtkpRepo.FindAsync(t => t.Ptkpcode.Equals(model.Ptkpcode));
                if (existingItem == null)
                {
                    var item = _mapper.Map<TblPtkp>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.Now;
                    var addedItem = await _PtkpRepo.AddAsync(item);
                    return _mapper.Map<PtkpDto>(addedItem);
                }
                else
                {
                    _mapper.Map(model, existingItem);
                    existingItem.UpdatedBy = _context.Username;
                    existingItem.UpdatedDate = DateTime.Now;
                    var updatedItem = await _PtkpRepo.UpdateAsync(existingItem);
                    return _mapper.Map<PtkpDto>(updatedItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Ptkp");
                throw;
            }
        }
    }
}
