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
    public class ShiftService : IShiftService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TblShift> _ShiftRepo;
        private readonly IGenericRepository<TblEmployeeShiftSchedule> _ShiftScheduleRepo;
        private readonly IGenericRepository<TblEmployee> _empRepo;
        private readonly IGenericRepository<TblShiftPattern> _ShiftPatternRepo;
        private readonly IGenericRepository<TblShiftPatternDetail> _ShiftPatternDetailRepo;
        private readonly ILogger<ShiftService> _logger;
        private readonly IContextService _context;
        private readonly IGenericRepository<VwEmployeeMonthlySchedule> _monthlyScheduleViewRepo;
        public ShiftService(
            ILogger<ShiftService> logger,
            IGenericRepository<TblShift> ShiftRepo,
            IContextService context, IGenericRepository<TblEmployee> empRepo,
            IGenericRepository<TblEmployeeShiftSchedule> shiftScheduleRepo,
            IGenericRepository<TblShiftPattern> ShiftPatternRepo,
            IGenericRepository<TblShiftPatternDetail> ShiftPatternDetailRepo,
            IGenericRepository<VwEmployeeMonthlySchedule> monthlyScheduleViewRepo,
            IMapper mapper)
        {
            _empRepo = empRepo;
            _logger = logger;
            _ShiftRepo = ShiftRepo;
            _ShiftScheduleRepo = shiftScheduleRepo;
            _mapper = mapper;
            _context = context;
            _ShiftPatternRepo = ShiftPatternRepo;
            _ShiftPatternDetailRepo = ShiftPatternDetailRepo;
            _monthlyScheduleViewRepo = monthlyScheduleViewRepo;
        }

        public async Task GenerateAsync(GenerateScheduleRequest request)
        {

            var employees = await _empRepo.GetListAsync();

            var daysInMonth = DateTime.DaysInMonth(request.Year, request.Month);

            foreach (var employee in employees)
            {
                for (int day = 1; day <= daysInMonth; day++)
                {
                    DateTime workDate = new DateTime(request.Year, request.Month, day);

                    int shiftId = await DetermineShiftAsync(employee, workDate, request.PatternId);

                    await _ShiftScheduleRepo.AddAsync(
                        new TblEmployeeShiftSchedule
                        {
                            EmployeeId = employee.EmployeeId,
                            WorkDate = workDate,
                            ShiftId = shiftId
                        });
                }
            }
        }

        private async Task<int> DetermineShiftAsync(TblEmployee employee, DateTime date, int patternId)
        {
            // 1. Get pattern details (ordered)
            var patternDetails = await _ShiftPatternDetailRepo.GetListAsync(x => x.PatternId == patternId);

            patternDetails = patternDetails.OrderBy(x => x.SequenceNo).ToList(); // Ensure correct order

            if (patternDetails == null || patternDetails.Count == 0)
                return employee.DefaultShift ?? 0;

            // 2. Determine cycle length
            int cycleLength = patternDetails.Count;

            // 3. Define cycle start (important for consistency)
            DateTime startDate = employee.HireDate ?? date;

            int dayOffset = (date.Date - startDate.Date).Days;

            if (dayOffset < 0)
                dayOffset = 0;

            // 4. Get position in cycle
            int index = dayOffset % cycleLength;

            // 5. Get shift from pattern
            var shift = patternDetails[index];

            return shift.ShiftId ?? employee.DefaultShift ?? 0;
        }

        public async Task<List<ShiftDto>> GetListAsync()
        {
            var entities = await _ShiftRepo.GetListAsync();
            return _mapper.Map<List<ShiftDto>>(entities);
        }

        public async Task<ShiftDto?> GetByIdAsync(int id)
        {
            var entity = await _ShiftRepo.GetFirstOrDefaultAsync(x =>
                    x.Id == id);

            return entity == null
                ? null
                : _mapper.Map<ShiftDto>(entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _ShiftRepo.FindAsync(x =>
                x.Id == id);
            if (entity == null)
                return 0;

            return await _ShiftRepo.Remove(entity);
        }

        public async Task<PagedResponse<EmployeeMonthlyScheduleDto>> GetPagedAsync(ScheduleDataTableRequest model)
        {
            try
            {
                var entityResult = await _monthlyScheduleViewRepo.GetPagedAsync(model);
                return entityResult.MapPaged<VwEmployeeMonthlySchedule, EmployeeMonthlyScheduleDto>(_mapper, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged Employees");
                throw;
            }
        }



        public async Task<ShiftDto> SaveAsync(ShiftDto model)
        {
            try
            {
                var existingItem = await _ShiftRepo.FindAsync(t => t.Id.Equals(model.Id));
                if (existingItem == null)
                {
                    var item = _mapper.Map<TblShift>(model);
                    item.CreatedBy = _context.Username;
                    item.CreatedDate = DateTime.Now;
                    var addedItem = await _ShiftRepo.AddAsync(item);
                    return _mapper.Map<ShiftDto>(addedItem);
                }
                else
                {
                    _mapper.Map(model, existingItem);
                    existingItem.UpdatedBy = _context.Username;
                    existingItem.UpdatedDate = DateTime.Now;
                    var updatedItem = await _ShiftRepo.UpdateAsync(existingItem);
                    return _mapper.Map<ShiftDto>(updatedItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Shift");
                throw;
            }
        }
    }
}
