using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Data;
using WebApplication3.DTO;
using WebApplication3.Model;
using WebApplication3.Utils;

public class UserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public UserMiddleware(RequestDelegate next, IConfiguration config, IMapper mapper)
    {
        _next = next;
        _config = config;
        _mapper = mapper;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Cookies["AccessToken"];

        if (!string.IsNullOrEmpty(token))
        {
            var user = await GetUserFromToken(token, context);
            var userResponse = _mapper.Map<UserResponseDTO>(user);

            if (user != null)
            {
                context.Items["User"] = userResponse; 
            }
            else
            {
                //Console.WriteLine("Token validation failed. Removing invalid token.");
                //context.Response.Cookies.Delete("AccessToken"); 

                //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //await context.Response.WriteAsync("Unauthorized: Invalid or expired token.");
                //return;

                //Console.WriteLine("Token validation failed. Removing invalid token.");


                context.Response.Cookies.Delete("AccessToken");

                var response = new ApiResponse<string>(
                    401,
                    "Unauthorized: Invalid or expired token."
                );

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json"; 
                await context.Response.WriteAsJsonAsync(response); 

                return; 
            }
        }

        await _next(context);
    }

    private async Task<UserModel?> GetUserFromToken(string token, HttpContext context)
    {
        try
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]);
            var tokenParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = jwtHandler.ValidateToken(token, tokenParams, out SecurityToken validatedToken);
            var userId = principal.FindFirst(ClaimTypes.Name)?.Value;

            if (userId == null)
            {
                Console.WriteLine("User ID not found in token.");
                return null;
            }

            Console.WriteLine($"Extracted User ID: {userId}");

            var dbContext = context.RequestServices.GetRequiredService<MobileDbContext>();
            return await dbContext.Users.FindAsync(int.Parse(userId));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return null;
        }
    }
}
