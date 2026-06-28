using App.Application.Interfaces.Services.Masters;
using App.Application.Interfaces.Services.Payroll;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
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
                int employeeId = 0;
                int.TryParse(code, out employeeId);
                if (employeeId == 0)
                    return BadRequest("Employee code is required");

                var EmployeeSalary = await _EmployeeSalaryService.GetByCodeAsync(employeeId);
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
            int employeeId = 0;
            int.TryParse(code, out employeeId);
            if (employeeId == 0)
                return BadRequest("Employee code is required");

            try
            {
                var result = await _EmployeeSalaryService.DeleteAsync(employeeId);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"EmployeeSalary {employeeId} deleted successfully"));

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
                if (request.Item.Count == 0)
                {
                    throw new Exception("Please select employee.");
                }

                var Employees = await _EmployeeService.GetListAsync();
                List<EmployeeSalaryDetailDto> details = new List<EmployeeSalaryDetailDto>();

                for (var i = 0; i < request.Item.Count; i++)
                {
                    var employeeItem = request.Item[i];
                    var employee = Employees.Where(t => t.EmployeeId == employeeItem.EmployeeId).FirstOrDefault();
                    if (employeeItem.EffectiveDate < DateTime.Now)
                    {
                        throw new Exception($"Invalid effective date for employee {employee.FullName}. Effective date must greater then today.");
                    }
                    foreach (var component in request.Components)
                    {
                        details.Add(new EmployeeSalaryDetailDto()
                        {
                            ComponentCode = component.ComponentCode,
                            Amount = component.DefaultAmount.GetValueOrDefault(),
                            EmployeeId = employeeItem.EmployeeId
                        });
                    }

                }

                var result = await _EmployeeSalaryService.SaveAsync(request.Item, details);
                if (result)
                {
                    return Json(ActionResponse.Ok("EmployeeSalary saved successfully"));
                }

                return Json(ActionResponse.Fail("EmployeeSalary saved failed"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        #endregion

    }
}
