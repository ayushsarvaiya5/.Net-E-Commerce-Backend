
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using WebApplication3.DTO;
using WebApplication3.Model;
using WebApplication3.Services;
using WebApplication3.Utils;
using WebApplication3.Interfaces;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public AuthController(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel userModel)
        {
            try
            {
                var responseDTO = await _authService.SignUpService(userModel);

                return Ok(new ApiResponse<UserResponseDTO>(201, responseDTO, "User Created Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin_DTO user)
        {
            try
            {
                var (responseDTO, tokenString) = await _authService.LoginService(user);

                // Store token in HttpOnly cookie
                Response.Cookies.Append("AccessToken", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "Login successful, token set in cookies"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }

        }

        [HttpPost("Logout")]
        public async Task<IActionResult> logout()
        {
            Response.Cookies.Delete("AccessToken");
            return Ok(new ApiResponse<string>(200, "Logout successful"));
        }
    }
}

