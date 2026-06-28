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

        [HttpGet]
        public async Task<IActionResult> GetList(
    string? departmentCode,
    string? positionCode,
    string? employeeId)
        {
            int id = 0;
            int.TryParse(employeeId, out id);
            var employees = await _EmployeeSalaryService.GetListAsync(
                departmentCode, positionCode, id);

            return Json(employees);
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
        public async Task<IActionResult> Add(EmployeeSalaryCreateModel request)
        {
            try
            {
                //List<EmployeeSalaryDto> items = new List<EmployeeSalaryDto>();
                //foreach (var employeeId in request.EmployeeIds)
                //{

                //}

                //var result = await _EmployeeSalaryService.Save(model.Item);
                //return (result != null) ? Json(ActionResponse.Ok("EmployeeSalary saved successfully")) : Json(ActionResponse.Fail("EmployeeSalary saved failed"));
                return Json(ActionResponse.Ok("EmployeeSalary saved successfully"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> Add([FromBody] EmployeeSalaryCreateModel request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            message = "Invalid request."
        //        });
        //    }

        //    if (request.EmployeeSalaries == null || !request.EmployeeSalaries.Any())
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            message = "Please select at least one employee."
        //        });
        //    }

        //    if (request.Components == null || !request.Components.Any())
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            message = "No salary components found."
        //        });
        //    }

        //    try
        //    {
        //        await _employeeSalaryService.AddAsync(request);

        //        return Json(new
        //        {
        //            success = true,
        //            message = "Employee salary saved successfully."
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error saving employee salary.");

        //        return Json(new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //}
        #endregion

    }
}
