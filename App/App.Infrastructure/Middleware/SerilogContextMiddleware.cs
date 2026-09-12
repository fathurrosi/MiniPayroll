using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace App.Infrastructure.Middleware
{
    public class SerilogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public SerilogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var machineName = Environment.MachineName;

            var username = context.User?.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "Anonymous";

            var ipAddress = context.Connection.RemoteIpAddress?.ToString()
                    ?? "Unknown";

            using (LogContext.PushProperty("MachineName", machineName))
            using (LogContext.PushProperty("Username", username))
            using (LogContext.PushProperty("IPAddress", ipAddress))
            {
                await _next(context);
            }
        }
    }
}
