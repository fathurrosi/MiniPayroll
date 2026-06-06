using App.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Components
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        //private readonly IContextService _contextService;
        //public BreadcrumbViewComponent(IContextService userService)
        //{
        //    _contextService = userService;
        //}
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Views/Shared/Layout/Breadcrumb.cshtml"); 
        }
    }

}
