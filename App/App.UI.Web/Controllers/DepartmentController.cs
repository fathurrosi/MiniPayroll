using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _DepartmentService;
        private readonly ILogger<DepartmentController> _logger;
        public DepartmentController(IDepartmentService DepartmentService, ILogger<DepartmentController> logger)

        {
            _DepartmentService = DepartmentService;
            _logger = logger;
        }

        #region Department
        public IActionResult Index()
        {
            var model = new PageModel<DepartmentDto>() { Title = "Department" };
            model.Item = new DepartmentDto();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _DepartmentService.GetPagedAsync(model);

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
                var model = await _DepartmentService.GetByCodeAsync(code);
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
                return BadRequest("model ID is required");

            try
            {
                var result = await _DepartmentService.DeleteAsync(code);
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
        public async Task<IActionResult> Add(PageModel<DepartmentDto> model)
        {
            try
            {
                // 1. Guard Clause
                if (model?.Item == null)
                {
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));
                }

                // 2. Model Validation (Returns immediately without throwing expensive exceptions)
                if (string.IsNullOrWhiteSpace(model.Item.DepartmentCode))
                {
                    return Json(ActionResponse.Fail("Please select a valid Department Code."));
                }

                if (string.IsNullOrWhiteSpace(model.Item.DepartmentName))
                {
                    return Json(ActionResponse.Fail("Department name is mandatory."));
                }

                // 3. Prevent Duplicate Departments on the Same Date
                if (model.Mode == FormMode.Create)
                {
                    // Pro-Tip: Change your service to look up by Code instead of ID for creation checks
                    var existingItem = await _DepartmentService.GetByCodeAsync(model.Item.DepartmentCode);
                    if (existingItem != null)
                    {
                        return Json(ActionResponse.Fail($"A Department already exists on this date: {existingItem.DepartmentName}"));
                    }
                }

                // 4. Execute Save Operation
                var result = await _DepartmentService.SaveAsync(model.Item);

                return (result != null)
                    ? Json(ActionResponse.Ok("Department saved successfully."))
                    : Json(ActionResponse.Fail("Failed to save the Department."));
            }
            catch (Exception ex)
            {
                // If you installed Serilog earlier, make sure to log the actual stack trace here:
                _logger.LogError(ex, "Error occurred while saving Department");

                return Json(ActionResponse.Fail($"Internal server error: {ex.Message}"));
            }
        }

        #endregion

    }
}
