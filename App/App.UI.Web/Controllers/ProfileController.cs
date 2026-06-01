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
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        #region Profile
        public IActionResult Index()
        {
            var item = new PageModel<ProfileDto>() { Title = "Profile" };
            item.Item = new ProfileDto();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] DataTableRequest model)
        {
            try
            {
                var result = await _profileService.GetPagedAsync(model);

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
                var Profile = await _profileService.GetByIdAsync(id);
                return Json(Profile);
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
                return BadRequest("Profile ID is required");

            try
            {
                var result = await _profileService.DeleteAsync(id);
                if (result > 0)
                    return Ok(ActionResponse.Ok($"Profile {id} deleted successfully"));

                return Ok(ActionResponse.Fail("Failed to delete Profile"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }

        [HttpPost] 
        public async Task<IActionResult> Add(PageModel<ProfileDto> model)
        {
            try
            {
                if (model.Mode == FormMode.Create)
                {
                    var existingItem = await _profileService.GetByIdAsync(model.Item.CompanyProfileId);
                    if (existingItem != null) return Json(ActionResponse.Fail($"Profile {model.Item.CompanyProfileId} already exist!"));
                }
                var result = await _profileService.SaveAsync(model.Item);
                return (result != null) ? Json(ActionResponse.Ok("Profile saved successfully")) : Json(ActionResponse.Fail("Profile saved failed"));
            }
            catch (Exception ex)
            {
                return Json(ActionResponse.Fail(ex.Message));
            }
        }


        #endregion

    }
}
