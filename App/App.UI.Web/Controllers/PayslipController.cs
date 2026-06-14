using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Payroll;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class PayslipController : BaseController
    {
        private readonly IPayslipService _PayslipService;
        public PayslipController(IPayslipService PayslipService)
        {
            _PayslipService = PayslipService;
        }

        #region Payslip
        public IActionResult Index()
        {
            var model = new PageModel<PayslipDto>() { Title = "Payslip" };
            model.Item = new PayslipDto();
            return View(model);
        }

        public async Task<IActionResult> Info(int id)
        {
            var model = new PageModel<PayslipDto>() { Title = "Payslip" };
            var item = await _PayslipService.GetByIdAsync(id);
            if (item == null)
                item = new PayslipDto();
            model.Item = item;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _PayslipService.GetPagedAsync(model);

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
                var model = await _PayslipService.GetByIdAsync(id);
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
                var result = await _PayslipService.DeleteAsync(id);
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
        public async Task<IActionResult> Add(PageModel<PayslipDto> model)
        {
            try
            {
                if (model.Mode == FormMode.Create)
                {
                    var existingItem = await _PayslipService.GetByIdAsync(model.Item.CompanyPayslipId);
                    if (existingItem != null) return Json(ActionResponse.Fail($"model {model.Item.CompanyPayslipId} already exist!"));
                }
                var result = await _PayslipService.SaveAsync(model.Item);
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
