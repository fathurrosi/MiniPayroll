using App.Application.Interfaces.Services.Masters;
using App.Application.Interfaces.Services.Payroll;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Services.Masters;
using App.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class EmployeeSalaryController : BaseController
    {
        private readonly IEmployeeService _EmployeeService;

        private readonly ISalaryComponentService _SalaryComponentService;
        private readonly IEmployeeSalaryService _EmployeeSalaryService;
        private readonly IPtkpService _PtkpService;
        private readonly IDepartmentService _departmentService;

        private readonly IPositionService _positionService;
        public EmployeeSalaryController(IEmployeeSalaryService EmployeeSalaryService
            , IPtkpService ptkpService
            , IEmployeeService employeeService
            , IDepartmentService departmentService
            , IPositionService positionService
            , ISalaryComponentService salaryComponentService)
        {
            _EmployeeSalaryService = EmployeeSalaryService;
            _PtkpService = ptkpService;
            _EmployeeService = employeeService;
            _SalaryComponentService = salaryComponentService;
            _departmentService = departmentService;
            _positionService = positionService;
        }

        #region EmployeeSalary
        public async Task<IActionResult> Index()
        {
            var model = new EmployeeSalaryModel() { Title = "Employee Salary" };

            model.Item = new List<EmployeeSalaryDto>();
            model.Employees = await _EmployeeService.GetListAsync();
            model.Components = await _SalaryComponentService.GetListAsync();
            model.Departments = await _departmentService.GetListAsync();
            model.Positions = await _positionService.GetListAsync();
            return View(model);
        }

        public async Task<IActionResult> Detail()
        {
            var model = new EmployeeSalaryCreateModel() { Title = "Add Employee Salary" };

            model.Item = new List<EmployeeSalaryDto>();
            model.Employees = await _EmployeeService.GetListAsync();
            model.Components = await _SalaryComponentService.GetListAsync();
            model.Departments = await _departmentService.GetListAsync();
            model.Positions = await _positionService.GetListAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] EmployeeSalaryDataTableRequest model)
        {
            try
            {
                var result = await _EmployeeSalaryService.GetPagedAsync(model);

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
                var EmployeeSalary = await _EmployeeSalaryService.GetByCodeAsync(code);
                return Json(EmployeeSalary);
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
                return BadRequest("EmployeeSalary code is required");

            try
            {
                var result = await _EmployeeSalaryService.DeleteAsync(code);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"EmployeeSalary {code} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete EmployeeSalary"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EmployeeSalaryCreateModel model)
        {
            try
            {
                //return Json(ActionResponse.Ok("EmployeeSalary saved successfully"));
                //if (model.Mode == FormMode.Create)
                //{
                //    var existingItem = await _EmployeeSalaryService.GetByCode(model.Item.EmployeeSalaryCode);
                //    if (existingItem != null) return Json(ActionResponse.Fail($"EmployeeSalary {model.Item.EmployeeSalaryCode} already exist!"));
                //}
                List<EmployeeSalaryDto> items = new List<EmployeeSalaryDto>();
                foreach (var employeeId in model.EmployeeIds)
                {
                    ////items.Add(new EmployeeSalaryDto()
                    ////{
                    ////    EmployeeId = employeeId,
                    ////    IsActive = true,
                    ////    EffectiveDate 
                    ////});
                }

                //var result = await _EmployeeSalaryService.Save(model.Item);
                //return (result != null) ? Json(ActionResponse.Ok("EmployeeSalary saved successfully")) : Json(ActionResponse.Fail("EmployeeSalary saved failed"));
                return Json(ActionResponse.Ok("EmployeeSalary saved successfully"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        #endregion

    }
}
