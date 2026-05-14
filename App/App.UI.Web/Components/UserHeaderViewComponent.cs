 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MANDe.Web.Components
{
    public class UserHeaderViewComponent : ViewComponent
    {
        //private readonly ICurrentUserService _userService;
        //public UserHeaderViewComponent(ICurrentUserService userService)
        //{
        //    _userService = userService;
        //}
        public async Task<IViewComponentResult> InvokeAsync()
        { 
            //UserDto item = await _userService.GetAsync();
            return View("~/Views/Shared/Layout/_Header.cshtml");
        }
    }

}
