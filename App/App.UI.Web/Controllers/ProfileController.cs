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
    public class ProfileController : BaseController
    {
        private readonly IEmployeeService _EmployeeService;
        public ProfileController(IEmployeeService employeeService)
        {
            _EmployeeService = employeeService;
        }

        #region Employee
        public IActionResult Index()
        {
            var item = new PageModel<EmployeeDto>() { Title = "Employee" };
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _EmployeeService.GetPagedAsync(model);

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
        public async Task<IActionResult> GetEmployee(string code)
        {
            try
            {
                var Employee = await _EmployeeService.GetByCode(code);
                return Json(Employee);
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }

        }


        [HttpDelete] 
        public async Task<IActionResult> DeleteEmployee(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Employee code is required");

            try
            {
                var result = await _EmployeeService.Delete(code);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"Employee {code} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete Employee"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost] 
        public async Task<IActionResult> AddEmployee(PageModel<EmployeeDto> model)
        {
            try
            {
                if (model.Mode == FormMode.Create)
                {
                    var existingItem = await _EmployeeService.GetByCode(model.Item.EmployeeCode);
                    if (existingItem != null) return Json(ActionResponse.Fail($"Employee {model.Item.EmployeeCode} already exist!"));
                }
                var result = await _EmployeeService.Save(model.Item);
                return (result != null) ? Json(ActionResponse.Ok("Employee saved successfully")) : Json(ActionResponse.Fail("Employee saved failed"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }


        #endregion

    }
}
