using App.Application.Interfaces.Services.Settings;
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
        private readonly IRoleService _RoleService;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleService RoleService, ILogger<RoleController> logger)

        {
            _RoleService = RoleService;
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
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _RoleService.GetPagedAsync(model);
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
                var model = await _RoleService.GetByCodeAsync(code);
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
                var result = await _RoleService.DeleteAsync(code);
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
                    var existingItem = await _RoleService.GetByCodeAsync(model.Item.RoleCode);
                    if (existingItem != null)
                    {
                        return Json(ActionResponse.Fail($"A Role already exists with this code: {existingItem.RoleCode}"));
                    }
                } 
                // 4. Execute Save Operation
                var result = await _RoleService.SaveAsync(model.Item);

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

    //    [HttpGet]
    //    public async Task<IActionResult> GetPermissions(int roleId)
    //    {
    //        var menus = await _menuService.GetAllMenus();
    //        var permissions =
    //            await _rolePermissionService.GetByRoleId(roleId);

    //        var result = menus
    //            .OrderBy(x => x.SortNo)
    //            .Select(menu =>
    //            {
    //                var permission =
    //                    permissions.FirstOrDefault(x =>
    //                        x.MenuId == menu.Id);

    //                return new MenuPermissionDto
    //                {
    //                    MenuId = menu.Id,
    //                    MenuName = menu.MenuName,
    //                    Level = menu.Level,
    //                    HasChildren = menu.HasChildren,

    //                    CanView = permission?.CanView ?? false,
    //                    CanCreate = permission?.CanCreate ?? false,
    //                    CanEdit = permission?.CanEdit ?? false,
    //                    CanDelete = permission?.CanDelete ?? false,
    //                    CanExport = permission?.CanExport ?? false,
    //                    CanApprove = permission?.CanApprove ?? false
    //                };
    //            })
    //            .ToList();

    //        return Json(result);
    //    }
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Save(
    //int roleId,
    //List<MenuPermissionDto> permissions)
    //    {
    //        try
    //        {
    //            if (roleId <= 0)
    //            {
    //                return Json(
    //                    ActionResponse.Fail(
    //                        "Invalid role."));
    //            }

    //            await _rolePermissionService.Save(
    //                roleId,
    //                permissions);

    //            return Json(
    //                ActionResponse.Ok(
    //                    "Permissions saved successfully."));
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(
    //                ActionResponse.Fail(
    //                    ex.Message));
    //        }
    //    }


        public async Task<IActionResult> AccessPermission()
        {
            var model = new RolePermissionViewModel
            {
                Roles = new List<RoleDto>(),
                MenuPermissions = new List<MenuPermissionDto>()
            };

            return View(model);
        }
    }
}
