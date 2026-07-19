using App.Application.Interfaces.Services;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Settings;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace App.UI.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _UserService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService UserService, ILogger<UserController> logger)

        {
            _UserService = UserService;
            _logger = logger;
        }

        #region User
        public async Task<IActionResult> Index()
        {
            var model = new PageModel<UserDto>() { Title = "User" };
            model.Item = new UserDto();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _UserService.GetPagedAsync(model);
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
                var model = await _UserService.GetByKey(code);
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
                var result = await _UserService.Delete(code);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PageModel<UserDto> model)
        {
            try
            {
                if (model?.Item == null)
                {
                    return Json(ActionResponse.Fail(
                        "Request data is missing or invalid."));
                }

                var item = model.Item;

                // Required validations
                if (string.IsNullOrWhiteSpace(item.Username))
                {
                    return Json(ActionResponse.Fail(
                        "Username is required."));
                }

                if (string.IsNullOrWhiteSpace(item.FullName))
                {
                    return Json(ActionResponse.Fail(
                        "Full name is required."));
                }

                if (model.Mode == FormMode.Create)
                {
                    if (string.IsNullOrWhiteSpace(item.Password))
                    {
                        return Json(ActionResponse.Fail(
                            "Password is required."));
                    }

                    if (string.IsNullOrWhiteSpace(item.ConfirmPassword))
                    {
                        return Json(ActionResponse.Fail(
                            "Confirm password is required."));
                    }

                    if (!string.Equals(
                            item.Password,
                            item.ConfirmPassword,
                            StringComparison.Ordinal))
                    {
                        return Json(ActionResponse.Fail(
                            "Password and confirm password do not match."));
                    }
                }

                // Optional email validation
                if (!string.IsNullOrWhiteSpace(item.Email))
                {
                    var emailValidator = new EmailAddressAttribute();

                    if (!emailValidator.IsValid(item.Email))
                    {
                        return Json(ActionResponse.Fail(
                            "Invalid email address."));
                    }
                }

                // Duplicate validation
                if (model.Mode == FormMode.Create)
                {
                    var existingUser =
                        await _UserService.GetByKey(item.Username);

                    if (existingUser != null)
                    {
                        return Json(ActionResponse.Fail(
                            $"Username '{item.Username}' already exists."));
                    }
                }
                else
                {
                    var existingUser =
                        await _UserService.GetByKey(item.Username);

                    if (existingUser != null &&
                        existingUser.Id != item.Id)
                    {
                        return Json(ActionResponse.Fail(
                            $"Username '{item.Username}' already exists."));
                    }
                }

                // Save
                var result = await _UserService.Save(item);

                if (result == null)
                {
                    return Json(ActionResponse.Fail(
                        "Failed to save user information."));
                }

                return Json(ActionResponse.Ok(
                    "User information has been saved successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred while saving user {Username}",
                    model?.Item?.Username);

                return Json(ActionResponse.Fail(
                    "An unexpected error occurred while saving the user."));
            }
        }

        #endregion

    }
}
