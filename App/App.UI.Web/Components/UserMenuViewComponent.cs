 
using Microsoft.AspNetCore.Mvc;

namespace MANDe.Web.Components
{

    public class UserMenuViewComponent : ViewComponent
    {
        //private readonly ICurrentUserService _userService;
        //public UserMenuViewComponent(ICurrentUserService userService)
        //{
        //    _userService = userService;
        //}
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //UserDto item = await _userService.GetAsync();
            //return View("~/Views/Shared/Layout/_SidebarUserMenu.cshtml", item);
            return View("~/Views/Shared/Layout/_Sidebar.cshtml");
        }
    }
}
