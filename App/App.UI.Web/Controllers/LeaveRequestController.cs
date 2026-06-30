using App.Application.Interfaces.Services.Leave;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Leave;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Services.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    [Route("LeaveRequest")]
    public class LeaveRequestController : BaseController
    {
        private readonly ILeaveService _leaveRequestService;
        private readonly IEmployeeService _employeeService;
        private readonly IBranchService _branchService;
        private readonly IDepartmentService _departmentService;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILogger<LeaveRequestController> _logger;

        public LeaveRequestController(
            ILeaveService leaveRequestService,
            IEmployeeService employeeService,
            IBranchService branchService,
            IDepartmentService departmentService,
            ILeaveTypeService leaveTypeService,
            ILogger<LeaveRequestController> logger)
        {
            _leaveRequestService = leaveRequestService;
            _employeeService = employeeService;
            _branchService = branchService;
            _departmentService = departmentService;
            _leaveTypeService = leaveTypeService;
            _logger = logger;
        }

        #region Leave
        [HttpGet("/LeaveRequest")]
        [HttpGet("/LeaveRequest/Index")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            var model = new PageModel<LeaveDto>() { Title = "Leave Request" };
            model.Item = new LeaveDto();
            return View(model);
        }
        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _leaveRequestService.GetPagedAsync(model);
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
        public async Task<IActionResult> GetData(long Id)
        {
            try
            {
                var result = await _leaveRequestService.GetByCodeAsync(Id);
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Leave Request with Id {Id}", Id);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long Id)
        {
            try
            {
                var result = await _leaveRequestService.DeleteAsync(Id);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"Leave Request deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete Leave Request"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        [HttpPost("/LeaveRequest/SaveAsync")]
        public async Task<IActionResult> SaveAsync(PageModel<LeaveDto> model)
        {
            try
            {
                if (model?.Item == null)
                {
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));
                }
                if (string.IsNullOrWhiteSpace(model.Item.BranchCode))
                {
                    return Json(ActionResponse.Fail("Please select a valid Branch."));
                }
                if (string.IsNullOrWhiteSpace(model.Item.DepartmentCode))
                {
                    return Json(ActionResponse.Fail("Please select a valid Department."));
                }
                if (model.Item.EmployeeId == 0)
                {
                    return Json(ActionResponse.Fail("Please select a valid Employee."));
                }
                if (string.IsNullOrWhiteSpace(model.Item.LeaveTypeCode))
                {
                    return Json(ActionResponse.Fail("Leave Type is mandatory."));
                }
                if (string.IsNullOrWhiteSpace(model.Item.Reason))
                {
                    return Json(ActionResponse.Fail("Reason is mandatory."));
                }
                
                var result = await _leaveRequestService.SaveAsync(model.Item);
                return (result != null)
                    ? Json(ActionResponse.Ok("Leave Request saved successfully."))
                    : Json(ActionResponse.Fail("Failed to save the Leave Request."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Leave Request with id {id}", model?.Item?.Id);
                return Json(ActionResponse.Fail(ex.Message));
            }
        }
        [HttpGet("GetBranchDropdown")]
        public async Task<IActionResult> GetBranchDropdown(string searchTerm)
        {
            var list = await _branchService.GetListAsync();
            var result = list
                .Where(x => string.IsNullOrEmpty(searchTerm) || 
                       (x.BranchName != null && x.BranchName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                       (x.BranchCode != null && x.BranchCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new { id = x.BranchCode, text = $"[{x.BranchCode}] {x.BranchName}" })
                .ToList();
            return Json(result);
        }

        [HttpGet("GetDepartmentDropdown")]
        public async Task<IActionResult> GetDepartmentDropdown(string searchTerm)
        {
            var list = await _departmentService.GetListAsync();
            var result = list
                .Where(x => string.IsNullOrEmpty(searchTerm) || 
                       (x.DepartmentName != null && x.DepartmentName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                       (x.DepartmentCode != null && x.DepartmentCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new { id = x.DepartmentCode, text = $"[{x.DepartmentCode}] {x.DepartmentName}" })
                .ToList();
            return Json(result);
        }

        [HttpGet("GetEmployeeDropdown")]
        public async Task<IActionResult> GetEmployeeDropdown(string searchTerm)
        {
            var list = await _employeeService.GetListAsync();
            var result = list
                .Where(x => string.IsNullOrEmpty(searchTerm) || 
                       (x.FullName != null && x.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                       (x.EmployeeCode != null && x.EmployeeCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new { id = x.EmployeeId, text = $"[{x.EmployeeCode}] {x.FullName}" })
                .ToList();
            return Json(result);
        }

        [HttpGet("GetLeaveTypeDropdown")]
        public async Task<IActionResult> GetLeaveTypeDropdown(string searchTerm)
        {
            var list = await _leaveTypeService.GetListAsync();
            var result = list
                .Where(x => string.IsNullOrEmpty(searchTerm) || 
                       (x.LeaveName != null && x.LeaveName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                       (x.LeaveCode != null && x.LeaveCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new { id = x.LeaveCode, text = $"[{x.LeaveCode}] {x.LeaveName}" })
                .ToList();
            return Json(result);
        }
        #endregion
    }
}
