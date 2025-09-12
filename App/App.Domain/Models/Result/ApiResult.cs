using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Domain.Models.Result
{
    public class ApiResult<T>
    {
        public ApiResult() { }
        public ApiResult(T data, bool isSuccess, string message)
        {
            Data = data;
            Success = isSuccess;
            Message = message;
        }
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }


        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

}
