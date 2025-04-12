using Microsoft.AspNetCore.Http;

namespace WebApplication3.Utils
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public ApiError(int statusCode, string message, List<string> errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors ?? new List<string>();
        }
    }
}
