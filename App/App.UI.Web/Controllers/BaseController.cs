using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.UI.Web.Controllers
{
    [Authorize]
    // aktifkan jika sudah di production
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class BaseController : Controller
    {
        protected bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        protected string Username =>
            User?.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous";

        protected string? UserId =>
            User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        protected string? DisplayName =>
            User?.FindFirst(ClaimTypes.GivenName)?.Value;

        protected string? Email =>
            User?.FindFirst(ClaimTypes.Email)?.Value;

        protected string? IpAddress =>
            HttpContext?.Connection?.RemoteIpAddress?.ToString();

        protected string? UserAgent =>
            Request?.Headers.UserAgent.ToString();

        /// <summary>
        /// Get claim value safely.
        /// </summary>
        protected string? GetClaimValue(string claimType) =>
            User?.FindFirst(claimType)?.Value;

        // Example:
        // protected Capability ClientCapability
        // {
        //     get
        //     {
        //         var capabilityValue = GetClaimValue(ClaimConstant.CLIENT_CAPABILITY)?
        //             .Replace(" ", "");
        //
        //         if (string.IsNullOrWhiteSpace(capabilityValue))
        //             return Capability.None;
        //
        //         return Enum.TryParse(capabilityValue, true, out Capability capability)
        //             ? capability
        //             : Capability.None;
        //     }
        // }
    }
}
