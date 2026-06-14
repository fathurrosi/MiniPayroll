using App.Application.Interfaces.Services.Masters;
using App.Application.Interfaces.Services.Payroll;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class SalaryComponentController : BaseController
    {
        private readonly ISalaryComponentService _salaryComponentService;
        public SalaryComponentController(ISalaryComponentService salaryComponentService)
        {
            _salaryComponentService = salaryComponentService;
        }

        #region SalaryComponent
        public IActionResult Index()
        {
            var model = new PageModel<SalaryComponentDto>() { Title = "Salary Component" };
            model.Item = new SalaryComponentDto();
            return View(model);
        } 

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _salaryComponentService.GetPagedAsync(model);

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
                var model = await _salaryComponentService.GetByCodeAsync(code);
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
            if (string.IsNullOrEmpty(code))
                return BadRequest("model code is required");

            try
            {
                var result = await _salaryComponentService.DeleteAsync(code);
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
        public async Task<IActionResult> Add(PageModel<SalaryComponentDto> model)
        {
            try
            {
                if (model.Mode == FormMode.Create)
                {
                    var existingItem = await _salaryComponentService.GetByCodeAsync(model.Item.ComponentCode);
                    if (existingItem != null) return Json(ActionResponse.Fail($"model {model.Item.ComponentCode} already exist!"));
                }
                var result = await _salaryComponentService.SaveAsync(model.Item);
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
