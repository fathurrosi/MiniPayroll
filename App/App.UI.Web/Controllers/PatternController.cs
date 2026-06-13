using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Services.Masters;
using App.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace App.UI.Web.Controllers
{
    public class PatternController : BaseController
    {
        private readonly ILogger<ShiftController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IShiftPatternService _ShiftPatternService;
        private readonly IShiftService _ShiftService;
        public PatternController(IShiftService ShiftService
            , IDepartmentService departmentService
            , IEmployeeService employeeService
            , IShiftPatternService ShiftPatternService
            , ILogger<ShiftController> logger)
        {
            _employeeService = employeeService;
            _ShiftService = ShiftService;
            _ShiftPatternService = ShiftPatternService;
            _departmentService = departmentService;
            _logger = logger;
        }

        #region Shift_Pattern
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("shift ID is required");

            try
            {
                var result = await _ShiftPatternService.DeleteAsync(id);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"shift {id} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete shift"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }


        public async Task<IActionResult> Index(int? year, int? month)
        {
            var model = new PageModel<ShiftPatternDto>() { Title = "Shift" };
            model.Item = new ShiftPatternDto();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ShiftPatternModel model)
        {
            try
            {



                await _ShiftPatternService.SaveAsync(model.Item);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            model.Shifts = await _ShiftService.GetListAsync();
            return View("Detail", model);
        }

        private bool CalculateWorkingHours(ShiftDto shift, out decimal workingTime)
        {
            workingTime = 0;
            // 1. Basic Validation
            if (shift == null || !shift.StartTime.HasValue || !shift.EndTime.HasValue)
            {
                throw new ArgumentException("Start time and End time are required.");
            }

            // Extract values from nullable types
            TimeOnly startTime = shift.StartTime.Value;
            TimeOnly endTime = shift.EndTime.Value;

            // 2. Calculate Total Shift Duration
            TimeSpan totalShiftDuration;

            // Handle Crosses Midnight / Overnight Logic
            if (shift.IsOvernight || endTime <= startTime)
            {
                // If it crosses midnight, math calculation spans across days
                totalShiftDuration = (TimeSpan.FromDays(1) - startTime.ToTimeSpan()) + endTime.ToTimeSpan();
            }
            else
            {
                totalShiftDuration = endTime - startTime;
            }

            TimeSpan totalBreakDuration = TimeSpan.Zero;

            // 3. Process and Validate Breaks (Optional fields)
            if (shift.BreakStart.HasValue && shift.BreakEnd.HasValue)
            {
                TimeOnly breakStart = shift.BreakStart.Value;
                TimeOnly breakEnd = shift.BreakEnd.Value;

                // Calculate break duration handling potential overnight shifts
                if (breakEnd < breakStart)
                {
                    totalBreakDuration = (TimeSpan.FromDays(1) - breakStart.ToTimeSpan()) + breakEnd.ToTimeSpan();
                }
                else
                {
                    totalBreakDuration = breakEnd - breakStart;
                }

                // Logical Check: Break timeline validity contextually matching shift
                // Note: For overnight shifts, absolute time boundaries can cross, 
                // but total duration checks protect data integrity.
                if (totalBreakDuration >= totalShiftDuration)
                {
                    throw new ArgumentException("Break duration must be shorter than the overall shift hours.");
                }
            }

            // 4. Final Net Working Hours Calculation
            TimeSpan netWorkingTime = totalShiftDuration - totalBreakDuration;

            if (netWorkingTime.TotalMinutes < 0)
            {
                throw new ArgumentException("Break duration cannot exceed total shift hours.");
            }

            // Convert to decimal and round to 2 decimal places to match your DTO property type
            workingTime = Math.Round((decimal)netWorkingTime.TotalHours, 2);

            return true;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var model = new ShiftPatternModel() { Title = "Shift Pattern" };

            var shiftPatteen = await _ShiftPatternService.GetByIdAsync(id);
            if (shiftPatteen == null)
            {
                shiftPatteen = new ShiftPatternDto();
                shiftPatteen.Details = new List<ShiftPatternDetailDto>();
            }

            model.Item = shiftPatteen;
            model.Shifts = await _ShiftService.GetListAsync();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _ShiftService.GetPagedPatternAsync(model);

                return Json(new
                {
                    draw = model.Draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalFilteredCount,
                    data = result.Items
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    draw = model.Draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = Array.Empty<object>()
                });
            }
        }

        #endregion

    }
}
