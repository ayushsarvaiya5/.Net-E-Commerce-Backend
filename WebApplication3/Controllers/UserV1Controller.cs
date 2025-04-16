
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Model;
using WebApplication3.DTO;
using WebApplication3.Data;
using WebApplication3.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Numerics;
using WebApplication3.Services;
using Microsoft.AspNetCore.RateLimiting;
using WebApplication3.Interfaces;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Tags("User")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/User")]
    public class UserV1Controller : ControllerBase
    {
        private readonly IUserService _userService;

        public UserV1Controller(IUserService userService)
        {
            _userService = userService;
        }

        [EnableRateLimiting("RateLimiter")]
        [Authorize]
        [HttpGet("Get-CurrentUser")]
        public IActionResult GetUser()
        {
            var user = HttpContext.Items["User"] as UserResponseDTO;
            if (user == null)
            {
                return Unauthorized(new ApiError(401, "Invalid token"));
            }

            Response.Headers["Deprecation"] = "Sun, 20 Apr 2025 00:00:00 GMT";
            Response.Headers["Link"] = "</api/v2/User/Get-CurrentUser>; rel=\"successor-version\"";
            Response.Headers["Warning"] = "299 - \"This API version will be removed after 20-04-2025. Use v2 instead.\"";

            return Ok(new ApiResponse<UserResponseDTO>(200, user));
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpGet("Get-All-Users")]
        public async Task<IActionResult> GetAllUsersV1()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(new ApiResponse<List<UserResponseDTO>>(200, users, "Fetching Successfull"));
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpGet("Get-UserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ApiError(400, "Email is required"));
            }

            var findUser = await _userService.GetUserByEmailAsync(email);

            if (findUser == null)
            {
                return NotFound(new ApiError(404, "User Not Found With given Email"));
            }

            return Ok(new ApiResponse<UserResponseDTO>(200, findUser, "Fetch Successfully"));
        }

        [Authorize]
        [HttpPatch("Update-User")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdate_DTO userModel)
        {
            var loggedInUser = HttpContext.Items["User"] as UserResponseDTO;
            int loggedInUserId = loggedInUser.Id;
            string loggedInUserRole = loggedInUser.Role;

            if ((loggedInUserRole != "Admin") && (loggedInUserId != userModel.Id))
            {
                return Forbid();
            }

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(userModel);
                return Ok(new ApiResponse<UserResponseDTO>(200, updatedUser, "User Successfully Updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPatch("Change-UserRole")]
        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRole_DTO request)
        {
            try
            {
                var updatedUser = await _userService.ChangeUserRoleAsync(request);
                return Ok(new ApiResponse<UserResponseDTO>(200, updatedUser, "Role updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("Delete-User/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok(new ApiResponse<string>(200, "User deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("Get-Users-ByRole/{role:alpha}")]
        public async Task<IActionResult> GetUserByRole(string role)
        {
            try
            {
                var users = await _userService.GetUsersByRoleAsync(role);
                return Ok(new ApiResponse<List<UserResponseDTO>>(200, users, "Fetching successful"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("Get-Users-Paginated")]
        public async Task<IActionResult> GetUsersPaginated(int page = 1, int pageSize = 2)
        {
            try
            {
                var (users, totalUsers) = await _userService.GetUsersPaginatedAsync(page, pageSize);

                return Ok(new ApiResponse<object>(
                    200,
                    new
                    {
                        Page = page,
                        PageSize = pageSize,
                        TotalUsers = totalUsers,
                        Users = users
                    },
                    "Pagination SUcessful"
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

    }
}
