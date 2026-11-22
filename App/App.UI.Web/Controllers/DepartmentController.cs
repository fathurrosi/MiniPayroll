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
                return BadRequest("Department code is required");

            try
            {
                var result = await _DepartmentService.DeleteAsync(code);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"Department {code} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete Department"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PageModel<DepartmentDto> model, [FromForm] DepartmentDto? item)
        {
            try
            {
                // 1. Resolve department item from model or direct item binding
                var department = model?.Item ?? item;
                if (department == null)
                {
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));
                }

                // 2. Validate fields
                if (string.IsNullOrWhiteSpace(department.DepartmentCode))
                {
                    return Json(ActionResponse.Fail("Department code is required."));
                }

                department.DepartmentCode = department.DepartmentCode.Trim().ToUpper();

                if (string.IsNullOrWhiteSpace(department.DepartmentName))
                {
                    return Json(ActionResponse.Fail("Department name is required."));
                }

                department.DepartmentName = department.DepartmentName.Trim();

                // 3. Check duplicate when creating
                var isEdit = model?.Mode == FormMode.Edit;
                var existingItem = await _DepartmentService.GetByCodeAsync(department.DepartmentCode);

                if (!isEdit && existingItem != null)
                {
                    return Json(ActionResponse.Fail($"Department with code '{department.DepartmentCode}' already exists ({existingItem.DepartmentName})."));
                }

                // 4. Save
                var result = await _DepartmentService.SaveAsync(department);

                return (result != null)
                    ? Json(ActionResponse.Ok(isEdit ? "Department updated successfully." : "Department created successfully."))
                    : Json(ActionResponse.Fail("Failed to save Department."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving Department");
                return Json(ActionResponse.Fail($"Internal server error: {ex.Message}"));
            }
        }

        [HttpPost("/Department/UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(string code, bool isActive)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return Json(ActionResponse.Fail("Invalid Department Code."));

                var existingItem = await _DepartmentService.GetByCodeAsync(code);
                if (existingItem == null)
                    return Json(ActionResponse.Fail("Department not found."));

                existingItem.IsActive = isActive;
                var result = await _DepartmentService.SaveAsync(existingItem);

                return (result != null)
                    ? Json(ActionResponse.Ok("Status updated successfully."))
                    : Json(ActionResponse.Fail("Failed to update status."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for Department {code}", code);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        #endregion

    }
}
