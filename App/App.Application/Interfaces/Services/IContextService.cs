using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Interfaces.Services
{
    public interface IContextService
    {
        string? Username { get; }

        bool IsAuthenticated { get; }
        string? IpAddress { get; }
        string? UserAgent { get; }
                  
        string Email { get; }
         
    }
}
