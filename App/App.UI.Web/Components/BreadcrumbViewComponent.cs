
using App.Application.Interfaces.Services;
using App.Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MANDe.Web.Components
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        private readonly IContextService _contextService;
        public BreadcrumbViewComponent(IContextService userService)
        {
            _contextService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // UserDto? item = await _contextService.GetUserAsync();
            //return View("~/Views/Shared/Layout/_Sitebar.cshtml", item);

            return View("~/Views/Shared/Layout/Breadcrumb.cshtml");
            //return View();
            //return View( );
        }
    }

}
