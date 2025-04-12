using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTO;
using WebApplication3.Interfaces;
using WebApplication3.Utils;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Tags("User")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/User")]
    public class UserV2Controller : ControllerBase
    {
        private readonly IUserService _userService;

        public UserV2Controller(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("Get-CurrentUser")]
        public IActionResult GetUser()
        {
            var user = HttpContext.Items["User"] as UserResponseDTO;
            if (user == null)
            {
                return Unauthorized(new ApiError(401, "Invalid token"));
            }

            var response = new UserV2ResponseDTO
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };

            return Ok(new ApiResponse<UserV2ResponseDTO>(200, response, "Fetched Successfully - V2"));
        }

    }
}
