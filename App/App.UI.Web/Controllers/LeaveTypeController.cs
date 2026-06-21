using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Services.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace App.UI.Web.Controllers
{
    //[ApiController]
    public class LeaveTypeController : BaseController
    {
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILogger<LeaveTypeController> _logger;
        public LeaveTypeController(ILeaveTypeService leaveTypeService, ILogger<LeaveTypeController> logger)
        {
            _leaveTypeService = leaveTypeService;
            _logger = logger;
        }

        #region LeaveType
        [HttpGet("/LeaveType")]
        [HttpGet("/LeaveType/Index")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            var model = new PageModel<LeaveTypeDto>() { Title = "Leave Type" };
            model.Item = new LeaveTypeDto();
            return View(model);
        }
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _leaveTypeService.GetPagedAsync(model);
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
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData(string code)
        {
            try
            {
                var result = await _leaveTypeService.GetByCodeAsync(code);
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving LeaveType with code {code}", code);
                //return StatusCode(500, new { error = "An error occurred while retrieving data." });
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("model ID is required");

            try
            {
                var result = await _leaveTypeService.DeleteAsync(code);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"model {code} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete model"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        [HttpPost("/LeaveType/SaveAsync")]
        public async Task<IActionResult> SaveAsync(PageModel<LeaveTypeDto> model)
        {
            try
            {
                // 1. Guard Clause
                if (model?.Item == null)
                {
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));
                }
                // 2. Model Validation (Returns immediately without throwing expensive exceptions)
                if (string.IsNullOrWhiteSpace(model.Item.LeaveCode))
                {
                    return Json(ActionResponse.Fail("Please select a valid Leave Code."));
                }
                if (string.IsNullOrWhiteSpace(model.Item.LeaveName))
                {
                    return Json(ActionResponse.Fail("Leave name is mandatory."));
                }
                // 3. Prevent Duplicate Departments on the Same Date
                if (model.Mode == FormMode.Create)
                {
                    // Pro-Tip: Change your service to look up by Code instead of ID for creation checks
                    var existingItem = await _leaveTypeService.GetByCodeAsync(model.Item.LeaveCode);
                    if (existingItem != null)
                    {
                        return Json(ActionResponse.Fail($"A Leave Type already exists with this code: {existingItem.LeaveName}"));
                    }
                }
                // 4. Execute Save Operation
                var result = await _leaveTypeService.SaveAsync(model.Item);
                return (result != null)
                    ? Json(ActionResponse.Ok("Leave Type saved successfully."))
                    : Json(ActionResponse.Fail("Failed to save the Leave Type."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving LeaveType with code {code}", model?.Item?.LeaveCode);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        #endregion
    }
}
