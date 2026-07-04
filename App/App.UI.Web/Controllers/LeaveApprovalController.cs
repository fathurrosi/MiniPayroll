using App.Application.Interfaces.Services.Leave;
using App.Application.Interfaces.Services.Masters;
using App.Domain.Enums;
using App.Domain.Models;
using App.Domain.Models.Dto.Masters;
using App.Domain.Models.Request;
using App.Domain.Models.Response;
using App.Infrastructure.Services.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.UI.Web.Controllers
{
    [Route("LeaveApproval")]
    public class LeaveApprovalController : BaseController
    {
        //private readonly ILeaveService _leaveRequestService;
        //private readonly ILogger<LeaveApprovalController> _logger;
        public IActionResult Index()
        {
            return View();
        }
    }
}
