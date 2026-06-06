using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class HolidayController : BaseController
    {
        private readonly IHolidayService _HolidayService;
        private readonly ILogger<HolidayController> _logger;
        public HolidayController(IHolidayService HolidayService, ILogger<HolidayController> logger)

        {
            _HolidayService = HolidayService;
            _logger = logger;
        }

        #region Holiday
        public IActionResult Index()
        {
            var model = new PageModel<HolidayDto>() { Title = "Holiday" };
            model.Item = new HolidayDto();
            return View(model);
        } 
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _HolidayService.GetPagedAsync(model);

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
                var model = await _HolidayService.GetByIdAsync(id);
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
                var result = await _HolidayService.DeleteAsync(id);
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
        public async Task<IActionResult> Add(PageModel<HolidayDto> model)
        {
            try
            {
                // 1. Guard Clause
                if (model?.Item == null)
                {
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));
                }

                // 2. Model Validation (Returns immediately without throwing expensive exceptions)
                if (model.Item.HolidayDate.Year <= 1900)
                {
                    return Json(ActionResponse.Fail("Please select a valid Holiday Date."));
                }

                if (string.IsNullOrWhiteSpace(model.Item.HolidayName))
                {
                    return Json(ActionResponse.Fail("Holiday name is mandatory."));
                }

                // 3. Prevent Duplicate Holidays on the Same Date
                if (model.Mode == FormMode.Create)
                {
                    // Pro-Tip: Change your service to look up by Date instead of ID for creation checks
                    var existingItem = await _HolidayService.GetByIdAsync(model.Item.Id);
                    if (existingItem != null)
                    {
                        return Json(ActionResponse.Fail($"A holiday already exists on this date: {existingItem.HolidayName}"));
                    }
                }

                // 4. Execute Save Operation
                var result = await _HolidayService.SaveAsync(model.Item);

                return (result != null)
                    ? Json(ActionResponse.Ok("Holiday saved successfully."))
                    : Json(ActionResponse.Fail("Failed to save the holiday."));
            }
            catch (Exception ex)
            {
                // If you installed Serilog earlier, make sure to log the actual stack trace here:
                _logger.LogError(ex, "Error occurred while saving holiday");

                return Json(ActionResponse.Fail($"Internal server error: {ex.Message}"));
            }
        }

        #endregion

    }
}
