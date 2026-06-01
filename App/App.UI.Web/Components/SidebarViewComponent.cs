
using App.Application.Interfaces.Services;
using App.Domain.Entities;
using App.Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MANDe.Web.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        public SidebarViewComponent(IUserService userService)
        {
            _userService = userService;
        } 
        public async Task<IViewComponentResult> InvokeAsync()
        {
            UserDto? item = await _userService.GetUserAsync();
            return View("~/Views/Shared/Layout/Sidebar.cshtml", item); 
        } 

    }
}
