using App.Application.Interfaces.Services;
using App.Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace App.UI.Web.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        public HeaderViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetUserAsync();
            return View("~/Views/Shared/Layout/Header.cshtml", user); 
        }
    }

}
