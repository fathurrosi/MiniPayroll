using App.Application.Interfaces.Services;
using App.Domain.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace App.UI.Web.Components
{
    public class FooterViewComponent : ViewComponent
    {
        //private readonly IContextService _contextService;
        //public FooterViewComponent(IContextService userService)
        //{
        //    _contextService = userService;
        //}
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Views/Shared/Layout/Footer.cshtml"); 
        }
    }

}
