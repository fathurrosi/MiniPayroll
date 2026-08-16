using App.Application.Interfaces.Services.Settings;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Dto.Settings;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        private readonly IMenuService _menuService;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleService roleService
            , IMenuService menuService
            , ILogger<RoleController> logger)

        {
            _menuService = menuService;
            _roleService = roleService;
            _logger = logger;
        }

        #region Role
        public async Task<IActionResult> Index()
        {
            var model = new PageModel<RoleDto>() { Title = "Role" };
            model.Item = new RoleDto();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SavePermissions( [FromBody] List<RolePermissionDto> permissions)
        {
            try
            {
                
                if (permissions == null || !permissions.Any())
                {
                    return Json(ActionResponse.Fail(
                        "No permissions to save."));
                }


                var result = await _roleService.SavePermissionAsync(permissions);

                return Json(ActionResponse.Ok("Permissions saved successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving role permissions");

                return Json(ActionResponse.Fail(
                    "An error occurred while saving permissions."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _roleService.GetPagedAsync(model);
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
                var model = await _roleService.GetByCodeAsync(code);
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
                return BadRequest("model code is required");

            try
            {
                var result = await _roleService.DeleteAsync(code);
                if (result != null)
                    return Ok(ActionResponse.Ok($"model {code} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete model"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(PageModel<RoleDto> model)
        {
            try
            {
                // 1. Guard Clause
                if (model?.Item == null)
                {
                    return Json(ActionResponse.Fail("Request data is missing or invalid."));
                }

                if (string.IsNullOrWhiteSpace(model.Item.RoleCode))
                {
                    return Json(ActionResponse.Fail("Overtime name is mandatory."));
                }

                if (string.IsNullOrWhiteSpace(model.Item.RoleName))
                {
                    return Json(ActionResponse.Fail("Overtime category is mandatory."));
                }

                // 3. Prevent Duplicate Roles on the Same Date
                if (model.Mode == FormMode.Create)
                {
                    // Pro-Tip: Change your service to look up by Date instead of ID for creation checks
                    var existingItem = await _roleService.GetByCodeAsync(model.Item.RoleCode);
                    if (existingItem != null)
                    {
                        return Json(ActionResponse.Fail($"A Role already exists with this code: {existingItem.RoleCode}"));
                    }
                }
                // 4. Execute Save Operation
                var result = await _roleService.SaveAsync(model.Item);

                return (result != null)
                    ? Json(ActionResponse.Ok("Role saved successfully."))
                    : Json(ActionResponse.Fail("Failed to save the Role."));
            }
            catch (Exception ex)
            {
                // If you installed Serilog earlier, make sure to log the actual stack trace here:
                _logger.LogError(ex, "Error occurred while saving Role");

                return Json(ActionResponse.Fail($"Internal server error: {ex.Message}"));
            }
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> GetPermissions(string roleCode, string menuId)
        {
            try
            {
                var permissions = await _roleService.GetPermissionsAsync(roleCode, menuId);
                return Json(ActionResponse.Ok(permissions.OrderBy(p => p.Sort)));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetParentManus()
        {
            try
            {
                var parenMenus = await _menuService.GetParentListAsync();
                return Json(ActionResponse.Ok(parenMenus));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }


        public async Task<IActionResult> AccessPermission()
        {
            var model = new RolePermissionViewModel
            {
                Roles = new List<RoleDto>(),
                MenuPermissions = new List<MenuPermissionDto>()
            };

            model.Roles = await _roleService.GetListAsync();
            model.Menus = await _menuService.GetParentListAsync();
            return View(model);
        }
    }
}
