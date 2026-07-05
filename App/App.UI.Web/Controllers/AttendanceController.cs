using App.Application.Interfaces.Services;
using App.Application.Interfaces.Services.Attendance;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto;
using App.Domain.Models.Dto.Attendance;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{

    public class AttendanceController : BaseController
    {

        private readonly IUserService _userService;
        private readonly IAttendanceService _AttendanceService;
        private readonly ILogger<AttendanceController> _logger;
        public AttendanceController(IAttendanceService AttendanceService
            , IUserService userService
            , ILogger<AttendanceController> logger)

        {
            _userService = userService;
            _AttendanceService = AttendanceService;
            _logger = logger;
        }

        #region Attendance
        public IActionResult Index()
        {
            var model = new PageModel<AttendanceDto>() { Title = "Attendance" };
            model.Item = new AttendanceDto();
            return View(model);
        }


        public async Task<ActionResult> Configuration()
        {
            var user = await _userService.GetUserAsync();
            var model = new PageModel<List<MenuDto>>() { Title = "Attendance Configuration" };
            model.Item = new List<MenuDto>();

            if (user != null)
                model.Item = user.ConfigMenuList.Where(t => t.ParentId == "ATT004").ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _AttendanceService.GetPagedAsync(model);

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
                var model = await _AttendanceService.GetByKeyAsync(id);
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
                var result = await _AttendanceService.DeleteAsync(id);
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
        public async Task<IActionResult> Add(PageModel<AttendanceDto> model)
        {
            try
            {
                // 1. Guard Clause
                if (model?.Item == null)
                {
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));
                }

                // 2. Model Validation (Returns immediately without throwing expensive exceptions)
                if (model.Item.AttendanceDate.Year <= 1900)
                {
                    return Json(ActionResponse.Fail("Please select a valid Attendance Date."));
                }

                if (string.IsNullOrWhiteSpace(model.Item.AttendanceStatus))
                {
                    return Json(ActionResponse.Fail("Attendance status is mandatory."));
                }

                // 3. Prevent Duplicate Attendances on the Same Date
                if (model.Mode == FormMode.Create)
                {
                    // Pro-Tip: Change your service to look up by Date instead of ID for creation checks
                    var existingItem = await _AttendanceService.GetByKeyAsync(model.Item.AttendanceId);
                    if (existingItem != null)
                    {
                        return Json(ActionResponse.Fail($"A Attendance already exists on this date: {existingItem.AttendanceDate}"));
                    }
                }

                // 4. Execute Save Operation
                var result = await _AttendanceService.SaveAsync(model.Item);

                return (result != null)
                    ? Json(ActionResponse.Ok("Attendance saved successfully."))
                    : Json(ActionResponse.Fail("Failed to save the Attendance."));
            }
            catch (Exception ex)
            {
                // If you installed Serilog earlier, make sure to log the actual stack trace here:
                _logger.LogError(ex, "Error occurred while saving Attendance");

                return Json(ActionResponse.Fail($"Internal server error: {ex.Message}"));
            }
        }

        #endregion

    }
}
