namespace WebApplication3.Utils
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(int statusCode, T data, string message = "Request successful")
        {
            StatusCode = statusCode;
            Data = data;
            Message = message;
        }
    }
}
