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
            if (!employees.Any()) return;

            var firstDate = request.DateFrom;// new DateTime(request.Year, request.Month, 1);
            var lastDate = request.DateTo;
            int daysInMonth = (lastDate - firstDate).Days + 1;

            // 1. Fetch Master Shifts into a Dictionary for instant O(1) lookups
            var shifts = await _ShiftRepo.GetListAsync();
            var shiftDict = shifts.ToDictionary(s => s.Id);

            // 2. Fetch Pattern Details ONCE outside the loop
            var rawPatternDetails = await _ShiftPatternDetailRepo.GetListAsync(x => x.PatternId == request.PatternId);
            var orderedPattern = rawPatternDetails.OrderBy(x => x.SequenceNo).ToList();
            int cycleLength = orderedPattern.Count;

            var schedules = new List<TblEmployeeShiftSchedule>(employees.Count * daysInMonth);

            foreach (var employee in employees)
            {
                // Define an Anchor Date. This is what the cycle mathematically rotates around.
                // Replace 'employee.JoinDate' with wherever you track the start of their pattern.
                // If not tracked, defaulting to the first of the month is a safe fallback.
                DateTime anchorDate = firstDate;

                for (int day = 1; day <= daysInMonth; day++)
                {

                    int shiftId = employee.DefaultShift ?? 0;
                    bool isRestDay = true;

                    // 3. Calculate the rotating sequence in memory
                    if (cycleLength > 0)
                    {
                        int dayOffset = (workDate.Date - anchorDate.Date).Days;

                        // Handle negative offsets gracefully if workDate is before anchorDate
                        if (dayOffset < 0)
                        {
                            int remainder = dayOffset % cycleLength;
                            dayOffset = remainder == 0 ? 0 : remainder + cycleLength;
                        }

                        int index = dayOffset % cycleLength;
                        shiftId = orderedPattern[index].ShiftId ?? employee.DefaultShift ?? 0;
                    }

                    // 4. Instant dictionary lookup instead of LINQ .Where()
                    if (shiftDict.TryGetValue(shiftId, out var shift))
                    {
                        isRestDay = shift.WorkingHours == 0;
                    }

                    schedules.Add(new TblEmployeeShiftSchedule
                    {
                        EmployeeId = employee.EmployeeId,
                        WorkDate = workDate,
                        ShiftId = shiftId,
                        IsRestDay = isRestDay
                    });
                }
            }

            // 5. Clean up existing schedules
            var existingSchedules = await _ShiftScheduleRepo.FindAllAsync(x =>
                x.WorkDate >= firstDate &&
                x.WorkDate < lastDate);

            if (existingSchedules.Any())
            {
                await _ShiftScheduleRepo.RemoveRangeAsync(existingSchedules);
            }

            // 6. Bulk Insert
            await _ShiftScheduleRepo.AddRangeAsync(schedules);
        }

        //public async Task GenerateAsync(GenerateScheduleRequest request)
        //{
        //    var employees = await _empRepo.GetListAsync();
        //    var shifts = await _ShiftRepo.GetListAsync();

        //    if (!employees.Any())
        //        return;

        //    var firstDate = new DateTime(request.Year, request.Month, 1);
        //    var lastDate = firstDate.AddMonths(1);

        //    int daysInMonth = DateTime.DaysInMonth(request.Year, request.Month);

        //    var schedules = new List<TblEmployeeShiftSchedule>(
        //        employees.Count * daysInMonth);

        //    foreach (var employee in employees)
        //    {
        //        for (int day = 1; day <= daysInMonth; day++)
        //        {
        //            var workDate = new DateTime(
        //                request.Year,
        //                request.Month,
        //                day);

        //            int shiftId = await DetermineShiftAsync(
        //                employee,
        //                workDate,
        //                request.PatternId);

        //            bool isRestDay = true;
        //            var shift = shifts.Where(t => t.Id == shiftId).FirstOrDefault();
        //            if (shift != null)
        //            {
        //                if (shift.ShiftName == "OFF")
        //                {
        //                    string test = shift.ShiftName;
        //                }
        //                isRestDay = shift.WorkingHours == 0;
        //            }
        //            schedules.Add(new TblEmployeeShiftSchedule
        //            {
        //                EmployeeId = employee.EmployeeId,
        //                WorkDate = workDate,
        //                ShiftId = shiftId,
        //                IsRestDay = isRestDay
        //            });
        //        }
        //    }

        //    var existingSchedules =
        //        await _ShiftScheduleRepo.FindAllAsync(x =>
        //            x.WorkDate >= firstDate &&
        //            x.WorkDate < lastDate);

        //    if (existingSchedules.Any())
        //    {
        //        await _ShiftScheduleRepo.RemoveRangeAsync(existingSchedules);
        //    }

        //    await _ShiftScheduleRepo.AddRangeAsync(schedules);
        //}

        //private async Task<int> DetermineShiftAsync(TblEmployee employee, DateTime date, int patternId)
        //{
        //    // 1. Get pattern details (ordered)
        //    var patternDetails = await _ShiftPatternDetailRepo.GetListAsync(x => x.PatternId == patternId);

        //    patternDetails = patternDetails.OrderBy(x => x.SequenceNo).ToList(); // Ensure correct order

        //    if (patternDetails == null || patternDetails.Count == 0)
        //        return employee.DefaultShift ?? 0;

        //    // 2. Determine cycle length
        //    int cycleLength = patternDetails.Count;

        //    DateTime startDate = date;
        //    int dayOffset = (date.Date - startDate.Date).Days;

        //    if (dayOffset < 0)
        //        dayOffset = 0;

        //    // 4. Get position in cycle
        //    int index = dayOffset % cycleLength;

        //    // 5. Get shift from pattern
        //    var shift = patternDetails[index];

        //    return shift.ShiftId ?? employee.DefaultShift ?? 0;
        //}

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

        {
            try
            {
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
