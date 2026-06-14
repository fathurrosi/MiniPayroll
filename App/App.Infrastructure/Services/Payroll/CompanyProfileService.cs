using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Payroll;
using App.Domain.Entities;
using App.Domain.Models.Dto.Payroll;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Extensions;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace App.Infrastructure.Services.Masters
{
    public class SalaryComponentService : ISalaryComponentService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblSalaryComponent> _SalaryComponentRepo;
        private readonly ILogger<SalaryComponentService> _logger;
        private readonly IContextService _context;
        public SalaryComponentService(
            ILogger<SalaryComponentService> logger,
           IGenericRepository<TblSalaryComponent> SalaryComponentRepo,
           IContextService context,
            IMapper mapper)
        {
            _logger = logger;
            _SalaryComponentRepo = SalaryComponentRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<SalaryComponentDto>> GetListAsync()
        {
            var entities = await _SalaryComponentRepo.GetListAsync();
            return _mapper.Map<List<SalaryComponentDto>>(entities);
        }

        public async Task<SalaryComponentDto?> GetByCodeAsync(string code)
        {
            var entity = await _SalaryComponentRepo.GetFirstOrDefaultAsync(x =>
                    x.ComponentCode == code);

            return entity == null
                ? null
                : _mapper.Map<SalaryComponentDto>(entity);
        }

        public async Task<int> DeleteAsync(string code)
        {
            var entity = await _SalaryComponentRepo.FindAsync(x =>
                x.ComponentCode == code);
            if (entity == null)
                return 0;

            return await _SalaryComponentRepo.Remove(entity);
        }

        public async Task<PagedResponse<SalaryComponentDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _SalaryComponentRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblSalaryComponent, SalaryComponentDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Employees");
                throw;
            }
        }



        public async Task<SalaryComponentDto> SaveAsync(SalaryComponentDto model)
        {
            try
            {
                var existingItem = await _SalaryComponentRepo.FindAsync(t => t.ComponentCode.Equals(model.ComponentCode));
                if (existingItem == null)
                {
                    var item = _mapper.Map<TblSalaryComponent>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.Now;
                    var addedItem = await _SalaryComponentRepo.AddAsync(item);
                    return _mapper.Map<SalaryComponentDto>(addedItem);
                }
                else
                {
                    _mapper.Map(model, existingItem);
                    existingItem.UpdatedBy = _context.Username;
                    existingItem.UpdatedDate = DateTime.Now;
                    var updatedItem = await _SalaryComponentRepo.UpdateAsync(existingItem);
                    return _mapper.Map<SalaryComponentDto>(updatedItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving SalaryComponent");
                throw;
            }
        }
    }
}
