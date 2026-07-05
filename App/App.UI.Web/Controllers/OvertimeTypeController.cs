using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace App.UI.Web.Controllers
{
    public class OvertimeTypeController : BaseController
    {
        private readonly IOvertimeTypeService _OvertimeTypeService;
        private readonly ILogger<OvertimeTypeController> _logger;
        public OvertimeTypeController(IOvertimeTypeService OvertimeTypeService, ILogger<OvertimeTypeController> logger)

        {
            _OvertimeTypeService = OvertimeTypeService;
            _logger = logger;
        }

        #region OvertimeType
        public IActionResult Index()
        {
            var model = new PageModel<OvertimeTypeDto>() { Title = "OvertimeType" };
            model.Item = new OvertimeTypeDto();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _OvertimeTypeService.GetPagedAsync(model); 
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
        public async Task<IActionResult> GetData(string code)
        {
            try
            {
                var model = await _OvertimeTypeService.GetByCodeAsync(code);
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
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("model code is required");

            try
            {
                var result = await _OvertimeTypeService.DeleteAsync(code);
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
        public async Task<IActionResult> Add(PageModel<OvertimeTypeDto> model)
        {
            try
            {
                // 1. Guard Clause
                if (model?.Item == null)
                {
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));
                }

                // 2. Model Validation (Returns immediately without throwing expensive exceptions)
                if (string.IsNullOrWhiteSpace(model.Item.OvertimeCode))
                {
                    return Json(ActionResponse.Fail("Overtime Code is mandatory."));
                }

                if (string.IsNullOrWhiteSpace(model.Item.OvertimeName))
                {
                    return Json(ActionResponse.Fail("Overtime name is mandatory."));
                }

                if (string.IsNullOrWhiteSpace(model.Item.OvertimeCategory))
                {
                    return Json(ActionResponse.Fail("Overtime category is mandatory."));
                }

                // 3. Prevent Duplicate OvertimeTypes on the Same Date
                if (model.Mode == FormMode.Create)
                {
                    // Pro-Tip: Change your service to look up by Date instead of ID for creation checks
                    var existingItem = await _OvertimeTypeService.GetByCodeAsync(model.Item.OvertimeCode);
                    if (existingItem != null)
                    {
                        return Json(ActionResponse.Fail($"A OvertimeType already exists with this code: {existingItem.OvertimeName}"));
                    }
                }

                //model.Item.IsWorkingDay = model.Item.OvertimeCategory == nameof(OvertimeCategory.WorkingDay);
                //model.Item.IsRestDay = model.Item.OvertimeCategory == nameof(OvertimeCategory.RestDay);
                //model.Item.IsPublicHoliday = model.Item.OvertimeCategory == nameof(OvertimeCategory.PublicHoliday);

                // 4. Execute Save Operation
                var result = await _OvertimeTypeService.SaveAsync(model.Item);

                return (result != null)
                    ? Json(ActionResponse.Ok("OvertimeType saved successfully."))
                    : Json(ActionResponse.Fail("Failed to save the OvertimeType."));
            }
            catch (Exception ex)
            {
                // If you installed Serilog earlier, make sure to log the actual stack trace here:
                _logger.LogError(ex, "Error occurred while saving OvertimeType");

                return Json(ActionResponse.Fail($"Internal server error: {ex.Message}"));
            }
        }

        #endregion

    }
}
