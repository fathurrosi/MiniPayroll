
using App.Application.Interfaces.Services;
using App.Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MANDe.Web.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IContextService _contextService;
        public HeaderViewComponent(IContextService userService)
        {
            _contextService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        { 
            return View("~/Views/Shared/Layout/Header.cshtml"); 
        }
    }

}
