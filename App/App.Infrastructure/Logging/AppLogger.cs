using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using App.Application.Interfaces.Repositories;


namespace App.Infrastructure.Logging
{
    public class AppLogger : ILogger
    {
        private string logFilePath = "POS.log";
        private readonly string _categoryName;
        private readonly ILogRepository _logRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppLogger(string categoryName, ILogRepository logRepository, IHttpContextAccessor httpContextAccessor)
        {
            _categoryName = categoryName;
            _logRepository = logRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            // This method is typically used to create a scope for logging, but for simplicity, we can return null.
            // In a real implementation, you might want to use a scope provider or similar mechanism.
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Error)
            {
                try
                {
                    var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                    string? username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                    if (string.IsNullOrEmpty(ipAddress))
                    {
                        ipAddress = "Unknown IP";
                    }

                    var logEntry = new Domain.Entities.Log
                    {
                        LogDate = DateTime.Now,
                        ComputerName = Environment.MachineName,
                        Ipaddress = ipAddress
                    };
                    logEntry.LogType = logLevel.ToString();
                    logEntry.LogMessage = exception is null ? formatter(state, exception) : $"{formatter(state, exception)} - Exception: {exception.Message}";
                    logEntry.Username = string.IsNullOrEmpty(username) ? "System" : username; // You might want to replace this with the actual username if available.
                    _logRepository.Create(logEntry);
                }
                catch (Exception)
                {
                    string logMessage = exception is null ? formatter(state, exception) : $"{formatter(state, exception)} - Exception: {exception.Message}";
                    WriteToFile(logMessage);
                }


            }
        }
        private void WriteToFile(string message)
        {
            try
            {
                // Append the log message with a timestamp to the file
                File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "Files", logFilePath), $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
