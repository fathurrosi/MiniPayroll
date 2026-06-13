using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response; 
using App.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class ScheduleController : BaseController
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IShiftPatternService _ShiftPatternService;
        private readonly IShiftService _ShiftService;
        public ScheduleController(IShiftService ShiftService
            , IDepartmentService departmentService
            , IEmployeeService employeeService
            , IShiftPatternService ShiftPatternService
            , ILogger<ScheduleController> logger)
        {
            _employeeService = employeeService;
            _ShiftService = ShiftService;
            _ShiftPatternService = ShiftPatternService;
            _departmentService = departmentService;
            _logger = logger;
        }
        
        #region CRUD_Shift_Schedule
        public async Task<IActionResult> Index(int? year, int? month)
        {
            int selectedYear = year ?? DateTime.Now.Year;
            int selectedMonth = month ?? DateTime.Now.Month;

            DateTime dateFrom = new DateTime(selectedYear, selectedMonth, 1);
            DateTime dateTo = dateFrom.AddMonths(1).AddDays(-1);
            var model = new ShiftScheduleModel
            {
                Year = year ?? DateTime.Now.Year,
                Month = month ?? DateTime.Now.Month,
                ModalDateFrom = dateFrom,
                ModalDateTo = dateTo,
                ShiftPatterns = await _ShiftPatternService.GetListAsync(),
                Departments = await _departmentService.GetListAsync(),
                Employees = await _employeeService.GetListAsync(),
                Title = "Employee Shift Schedule"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Generate(GenerateScheduleRequest request)
        {
            try
            {
                if (request.DateFrom > request.DateTo)
                {
                    throw new Exception("From Date cannot be greater than To Date.");
                }

                await _ShiftService.GenerateAsync(request);
                return Json(ActionResponse.Ok("Schedule generated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating schedule");
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] ScheduleDataTableRequest model)
        {
            try
            {
                var result = await _ShiftService.GetPagedEmplooyeeScheduleAsync(model);

                return Json(new
                {
                    draw = model.Draw,
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalFilteredCount,
                    data = result.Items
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating schedule");
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
