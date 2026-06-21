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
    public class HolidayService : IHolidayService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblHoliday> _holidayRepo;
        private readonly ILogger<HolidayService> _logger;
        private readonly IContextService _context;
        public HolidayService(
            ILogger<HolidayService> logger,
           IGenericRepository<TblHoliday> holidayRepo,
           IContextService context,
            IMapper mapper)
        {
            _logger = logger;
            _holidayRepo = holidayRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<HolidayDto>> GetListAsync()
        {
            var entities = await _holidayRepo.GetListAsync();
            return _mapper.Map<List<HolidayDto>>(entities);
        }

        public async Task<HolidayDto?> GetByIdAsync(int id)
        {
            var entity = await _holidayRepo.GetFirstOrDefaultAsync(x =>
                    x.Id == id);

            return entity == null
                ? null
                : _mapper.Map<HolidayDto>(entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _holidayRepo.FindAsync(x =>
                x.Id == id);
            if (entity == null)
                return 0;

            return await _holidayRepo.Remove(entity);
        }

        public async Task<PagedResponse<HolidayDto>> GetPagedAsync(DataTableRequest model)
        {
            try
            {
                var entityResult = await _holidayRepo.GetPagedAsync(model);
                return entityResult.MapPaged<TblHoliday, HolidayDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Employees");
                throw;
            }
        }



        public async Task<HolidayDto> SaveAsync(HolidayDto model)
        {
            try
            {
                var existingItem = await _holidayRepo.FindAsync(t => t.Id.Equals(model.Id));
                if (existingItem == null)
                {
                    var item = _mapper.Map<TblHoliday>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.Now;
                    var addedItem = await _holidayRepo.AddAsync(item);
                    return _mapper.Map<HolidayDto>(addedItem);
                }
                else
                {
                    _mapper.Map(model, existingItem);
                    existingItem.UpdatedBy = _context.Username;
                    existingItem.UpdatedDate = DateTime.Now;
                    var updatedItem = await _holidayRepo.UpdateAsync(existingItem);
                    return _mapper.Map<HolidayDto>(updatedItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Holiday");
                throw;
            }
        }
    }
}
