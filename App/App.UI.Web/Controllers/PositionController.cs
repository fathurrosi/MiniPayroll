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
    public class PositionController : BaseController
    {
        private readonly IPositionService _PositionService;
        private readonly ILogger<PositionController> _logger;
        public PositionController(IPositionService PositionService, ILogger<PositionController> logger)

        {
            _PositionService = PositionService;
            _logger = logger;
        }

        #region Position
        public IActionResult Index()
        {
            var model = new PageModel<PositionDto>() { Title = "Position" };
            model.Item = new PositionDto();
            return View(model);
        } 
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _PositionService.GetPagedAsync(model);

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
        public async Task<IActionResult> GetData(string  code)
        {
            try
            {
                var model = await _PositionService.GetByCodeAsync(code);
                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string code)
        {
            if (string.IsNullOrWhiteSpace(code) )
                return BadRequest("model ID is required");

            try
            {
                var result = await _PositionService.DeleteAsync(code);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"model {code} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete model"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PageModel<PositionDto> model)
        {
            try
            {
                // 1. Guard Clause
                if (model?.Item == null)
                {
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));
                }

                // 2. Model Validation (Returns immediately without throwing expensive exceptions)
                if (string.IsNullOrWhiteSpace(model.Item.PositionCode))
                {
                    return Json(ActionResponse.Fail("Please select a valid Position Code."));
                }

                if (string.IsNullOrWhiteSpace(model.Item.PositionName))
                {
                    return Json(ActionResponse.Fail("Position name is mandatory."));
                }

                // 3. Prevent Duplicate Positions on the Same Date
                if (model.Mode == FormMode.Create)
                {
                    // Pro-Tip: Change your service to look up by Code instead of ID for creation checks
                    var existingItem = await _PositionService.GetByCodeAsync(model.Item.PositionCode);
                    if (existingItem != null)
                    {
                        return Json(ActionResponse.Fail($"A Position already exists on this date: {existingItem.PositionName}"));
                    }
                }

                // 4. Execute Save Operation
                var result = await _PositionService.SaveAsync(model.Item);

                return (result != null)
                    ? Json(ActionResponse.Ok("Position saved successfully."))
                    : Json(ActionResponse.Fail("Failed to save the Position."));
            }
            catch (Exception ex)
            {
                // If you installed Serilog earlier, make sure to log the actual stack trace here:
                _logger.LogError(ex, "Error occurred while saving Position");

                return Json(ActionResponse.Fail($"Internal server error: {ex.Message}"));
            }
        }

        [HttpPost("/Position/UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(string code, bool isActive)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return Json(ActionResponse.Fail("Invalid Position Code."));

                var existingItem = await _PositionService.GetByCodeAsync(code);
                if (existingItem == null)
                    return Json(ActionResponse.Fail("Position not found."));

                existingItem.IsActive = isActive;
                var result = await _PositionService.SaveAsync(existingItem);

                return (result != null)
                    ? Json(ActionResponse.Ok("Status updated successfully."))
                    : Json(ActionResponse.Fail("Failed to update status."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for Position {code}", code);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        #endregion

    }
}
