using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Services.Masters;
using App.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class ShiftController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IShiftPatternService _ShiftPatternService;
        private readonly IShiftService _ShiftService;
        public ShiftController(IShiftService ShiftService
            , IDepartmentService departmentService
            ,IEmployeeService employeeService
            , IShiftPatternService ShiftPatternService)
        {
            _employeeService = employeeService;
            _ShiftService = ShiftService;
            _ShiftPatternService = ShiftPatternService;
            _departmentService = departmentService;
        }

        #region Shift

        [HttpPost]
        public async Task<IActionResult> GenerateSchedule(GenerateScheduleRequest request)
        {
            try
            {
                await _ShiftService.GenerateAsync(request);
                return Json(ActionResponse.Ok("Schedule generated successfully"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        public async Task<IActionResult> Index(int? year, int? month)
        {
            var model = new ShiftScheduleModel
            {
                Year = year ?? DateTime.Now.Year,
                Month = month ?? DateTime.Now.Month,
                ShiftPatterns = await _ShiftPatternService.GetListAsync(),
                Departments = await _departmentService.GetListAsync(),
                Employees = await _employeeService.GetListAsync(),
            };

            return View(model);
        }


        public async Task<IActionResult> Employee(int? year, int? month)
        {
            var model = new ShiftScheduleModel
            {
                Year = year ?? DateTime.Now.Year,
                Month = month ?? DateTime.Now.Month,
                ShiftPatterns = await _ShiftPatternService.GetListAsync(),
                Departments = await _departmentService.GetListAsync(),
                Employees = await _employeeService.GetListAsync(),
            };

            return View(model);
        }

        public async Task<IActionResult> Info(int id)
        {
            var model = new PageModel<ShiftDto>() { Title = "Shift" };
            var item = await _ShiftService.GetByIdAsync(id);
            if (item == null)
                item = new ShiftDto();
            model.Item = item;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] ScheduleDataTableRequest model)
        {
            try
            {
                var result = await _ShiftService.GetPagedAsync(model);

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

        [HttpGet]
        public async Task<IActionResult> GetData(int id)
        {
            try
            {
                var model = await _ShiftService.GetByIdAsync(id);
                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }

        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("model ID is required");

            try
            {
                var result = await _ShiftService.DeleteAsync(id);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"model {id} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete model"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PageModel<ShiftDto> model)
        {
            try
            {
                if (model.Mode == FormMode.Create)
                {
                    var existingItem = await _ShiftService.GetByIdAsync(model.Item.Id);
                    if (existingItem != null) return Json(ActionResponse.Fail($"model {model.Item.Id} already exist!"));
                }
                var result = await _ShiftService.SaveAsync(model.Item);
                return (result != null) ? Json(ActionResponse.Ok("model saved successfully")) : Json(ActionResponse.Fail("model saved failed"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }


        #endregion

    }
}
