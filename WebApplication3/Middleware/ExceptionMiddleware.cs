﻿using System.Net;
using System.Text.Json;

namespace WebApplication3.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed with request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception Occurred"); // Log the exception
                await HandleExceptionAsync(context); // Handle error response
            }
        }

        private static Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500

            var response = new
            {
                status = 500,
                message = "Internal Server Error" // Generic error message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}









//using System.Net;
//using System.Text.Json;
//using Serilog;

//namespace WebApplication3.Middleware
//{
//    public class ExceptionMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public ExceptionMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            try
//            {
//                await _next(context); // Proceed with request
//            }
//            catch (Exception ex)
//            {
//                // Log exception using Serilog (structured)
//                Log.Error(ex, "Unhandled Exception Occurred. Path: {Path}, Method: {Method}", context.Request.Path, context.Request.Method);

//                await HandleExceptionAsync(context); // Return 500 response
//            }
//        }

//        private static Task HandleExceptionAsync(HttpContext context)
//        {
//            context.Response.ContentType = "application/json";
//            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500

//            var response = new
//            {
//                status = 500,
//                message = "Internal Server Error" // Generic error message
//            };

//            var jsonResponse = JsonSerializer.Serialize(response);
//            return context.Response.WriteAsync(jsonResponse);
//        }
//    }
//}
