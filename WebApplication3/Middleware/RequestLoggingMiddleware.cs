using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebApplication3.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
                stopwatch.Stop();

                Log.Information("Request completed {@RequestDetails}", new
                {
                    Method = context.Request.Method,
                    Path = context.Request.Path,
                    StatusCode = context.Response.StatusCode,
                    QueryString = context.Request.QueryString.ToString(),
                    Timestamp = DateTime.UtcNow,
                    ElapsedTime = $"{stopwatch.ElapsedMilliseconds}ms"
                });
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Log.Error("Error processing request {@ErrorDetails}", new
                {
                    Method = context.Request.Method,
                    Path = context.Request.Path,
                    QueryString = context.Request.QueryString.ToString(),
                    ExceptionMessage = ex.Message,
                    ExceptionType = ex.GetType().Name,
                    StackTrace = ex.StackTrace,
                    Timestamp = DateTime.UtcNow,
                    ElapsedTime = $"{stopwatch.ElapsedMilliseconds}ms"
                });

                throw; // Exception will be handled by ExceptionHandlingMiddleware
            }
        }
    }
}
