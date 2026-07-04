using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.UI.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class EmployeeManagementController : BaseController
    {
        private readonly IDepartmentService _departmentService;
        private readonly IPositionService _positionService;
        private readonly IEmployeeService _EmployeeService;
        private readonly IPtkpService _PtkpService;
        private readonly ILogger<EmployeeManagementController> _logger;

        public EmployeeManagementController(
            IDepartmentService departmentService,
            IPositionService positionService,
            IEmployeeService employeeService,
            IPtkpService ptkpService,
            ILogger<EmployeeManagementController> logger)
        {
            _departmentService = departmentService;
            _positionService = positionService;
            _EmployeeService = employeeService;
            _PtkpService = ptkpService;
            _logger = logger;
        }

        #region Employee
        public async Task<IActionResult> Index()
        {
            //var item = new EmployeeModel() { Title = "Employee Management" };
            //item.PtkpList = await _PtkpService.GetListAsync();


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployees([FromBody] DataTableRequest model)
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

        [HttpGet("GetDepartmentDropdown")]
        public async Task<IActionResult> GetDepartmentDropdown(string searchTerm)
        {
            try
            {
                // Proteksi ekstra jika objek service ternyata tidak ter-resolve (null)
                if (_departmentService == null)
                {
                    return Json(Array.Empty<object>());
                }

                var allDepartments = await _departmentService.GetListAsync();

                // Jika data kosong atau null dari internal service, langsung return array kosong
                if (allDepartments == null || !allDepartments.Any())
                {
                    return Json(Array.Empty<object>());
                }

                var filteredDeparments = allDepartments.Where(p => p != null && p.IsActive);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    filteredDeparments = filteredDeparments.Where(p =>
                        p.DepartmentName != null && p.DepartmentName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    );
                }

                var result = filteredDeparments.Select(p => new
                {
                    id = p.DepartmentCode.ToString(),
                    text = p.DepartmentName ?? "Unknown Department"
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Department dropdown data");
                return Json(Array.Empty<object>());
            }
        }

        [HttpGet("GetPositionDropdown")]
        public async Task<IActionResult> GetPositionDropdown(string searchTerm)
        {
            try
            {
                // Proteksi ekstra jika objek service ternyata tidak ter-resolve (null)
                if (_positionService == null)
                {
                    return Json(Array.Empty<object>());
                }

                var allPositions = await _positionService.GetListAsync();

                // Jika data kosong atau null dari internal service, langsung return array kosong
                if (allPositions == null || !allPositions.Any())
                {
                    return Json(Array.Empty<object>());
                }

                var filteredPositions = allPositions.Where(p => p != null && p.IsActive);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    filteredPositions = filteredPositions.Where(p =>
                        p.PositionName != null && p.PositionName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    );
                }

                var result = filteredPositions.Select(p => new
                {
                    id = p.PositionCode.ToString(),
                    text = p.PositionName ?? "Unknown Position"
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Position dropdown data");
                return Json(Array.Empty<object>());
            }
        }
        #endregion

    }
}
