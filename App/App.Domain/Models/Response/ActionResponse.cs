
namespace App.Domain.Models.Response
{
    public class ActionResponse
    {
        public bool Success { get; init; }

        public string Message { get; init; } = string.Empty;

        public string? RedirectUrl { get; init; }

        public object? Data { get; init; }

        public static ActionResponse Ok(
            string message = "Operation completed successfully",
            object? data = null,
            string? redirectUrl = null)
            => new()
            {
                Success = true,
                Message = message,
                Data = data,
                RedirectUrl = redirectUrl
            };

        public static ActionResponse Fail(
            string message = "Operation failed",
            string? redirectUrl = null)
            => new()
            {
                Success = false,
                Message = message,
                RedirectUrl = redirectUrl
            };
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data)
            => new()
            {
                IsSuccess = true,
                Data = data
            };

        public static ApiResponse<T> Fail(string message)
            => new()
            {
                IsSuccess = false,
                Message = message
            };
    }
}
