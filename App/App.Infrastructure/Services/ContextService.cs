using App.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace App.Infrastructure.Services
{
    public class ContextService : IContextService
    {
        private readonly IHttpContextAccessor _accessor;

        public ContextService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        private HttpContext? HttpContext => _accessor.HttpContext;

        public bool IsAuthenticated =>
            HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        public string? Username =>
         HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

        public string? IpAddress =>
            HttpContext?.Connection?.RemoteIpAddress?.ToString();

        public string? UserAgent => HttpContext?.Request?.Headers["User-Agent"].ToString();

        public string Email => HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }
}
