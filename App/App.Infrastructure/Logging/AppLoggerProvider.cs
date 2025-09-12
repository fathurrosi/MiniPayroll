using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using App.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Logging
{
    public class AppLoggerProvider : ILoggerProvider
    {
        private readonly ILogRepository _logRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppLoggerProvider(ILogRepository logRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _logRepository = logRepository;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new AppLogger(categoryName, _logRepository, _httpContextAccessor);
        }

        public void Dispose()
        {
        }
    }
}
