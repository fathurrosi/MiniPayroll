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
    [Route("Branch")]
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;
        private readonly ILogger<BranchController> _logger;

        public BranchController(IBranchService branchService, ILogger<BranchController> logger)
        {
            _branchService = branchService;
            _logger = logger;
        }

        #region Branch
        [HttpGet("/Branch")]
        [HttpGet("/Branch/Index")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            var model = new PageModel<BranchDto>() { Title = "Branch" };
            model.Item = new BranchDto();
            return View(model);
        }

        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _branchService.GetPagedAsync(model);
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
                var result = await _branchService.GetByCodeAsync(code);
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Branch with code {code}", code);
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
                var result = await _branchService.DeleteAsync(code);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"model {code} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete model"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        [HttpPost("/Branch/SaveAsync")]
        public async Task<IActionResult> SaveAsync(PageModel<BranchDto> model)
        {
            try
            {
                // 1. Guard Clause
                if (model?.Item == null)
                {
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));
                }
                // 2. Model Validation (Returns immediately without throwing expensive exceptions)
                if (string.IsNullOrWhiteSpace(model.Item.BranchCode))
                {
                    return Json(ActionResponse.Fail("Please select a valid Branch Code."));
                }
                if (string.IsNullOrWhiteSpace(model.Item.BranchName))
                {
                    return Json(ActionResponse.Fail("Branch name is mandatory."));
                }
                // 3. Prevent Duplicate Branch on the Same Date
                if (model.Mode == FormMode.Create)
                {
                    // Pro-Tip: Change your service to look up by Code instead of ID for creation checks
                    var existingItem = await _branchService.GetByCodeAsync(model.Item.BranchCode);
                    if (existingItem != null)
                    {
                        return Json(ActionResponse.Fail($"A Branch already exists with this code: {existingItem.BranchName}"));
                    }
                }
                // 4. Execute Save Operation
                var result = await _branchService.SaveAsync(model.Item);
                return (result != null)
                    ? Json(ActionResponse.Ok("Branch saved successfully."))
                    : Json(ActionResponse.Fail("Failed to save the Branch."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Branch with code {code}", model?.Item?.BranchCode);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        [HttpPost("/Branch/UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(string code, bool isActive)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return Json(ActionResponse.Fail("Invalid Branch Code."));

                var existingItem = await _branchService.GetByCodeAsync(code);
                if (existingItem == null)
                    return Json(ActionResponse.Fail("Branch not found."));

                existingItem.IsActive = isActive;
                var result = await _branchService.SaveAsync(existingItem);

                return (result != null)
                    ? Json(ActionResponse.Ok("Status updated successfully."))
                    : Json(ActionResponse.Fail("Failed to update status."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for Branch {code}", code);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        #endregion
    }
}
