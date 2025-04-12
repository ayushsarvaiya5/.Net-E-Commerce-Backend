////ID: 1

//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using WebApplication3.Data;
//using WebApplication3.DTO;
//using WebApplication3.Model;
//using WebApplication3.Utils;

//namespace WebApplication3.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly MobileDbContext _db;
//        private readonly IMapper _mapper;

//        public AuthController(MobileDbContext db, IMapper mapper)
//        {
//            _db = db;
//            _mapper = mapper;
//        }

//        [HttpPost("SignUp")]
//        public async Task<IActionResult> CreateUser([FromBody] UserModel userModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest("Validation Failed");
//            }

//            var findUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);
//            if (findUser != null)
//            {
//                return BadRequest($"User with {userModel.Email} Email ID already exists");
//            }

//            userModel.Id = (await _db.Users.MaxAsync(u => u.Id)) + 1;
//            userModel.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userModel.PasswordHash, 13);

//            await _db.Users.AddAsync(userModel);
//            await _db.SaveChangesAsync();

//            UserResponseDTO responseDTO = _mapper.Map<UserResponseDTO>(userModel);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "User Created Successfully"));
//        }

//        [HttpPost("Login")]
//        public async Task<ActionResult<string>> Login([FromBody] UserLogin_DTO user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest("Validation failed");
//            }

//            var findUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
//            if (findUser == null)
//            {
//                return NotFound(new ApiError(404, "User Not Found"));
//            }

//            if (!BCrypt.Net.BCrypt.EnhancedVerify(user.PasswordHash, findUser.PasswordHash))
//            {
//                return BadRequest(new ApiError(400, "Invalid Password"));
//            }

//            // Authorization

//            var key = "supersecretkey123456123456123456";

//            var tokenHandler = new JwtSecurityTokenHandler();
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[]
//                {
//                        new Claim(ClaimTypes.Name, findUser.Id.ToString()),
//                        new Claim(ClaimTypes.Role, findUser.Role)
//                }),
//                Expires = DateTime.UtcNow.AddHours(1),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
//            };
//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            var tokenString = tokenHandler.WriteToken(token);

//            var responseDTO = _mapper.Map<UserLoginResponseDTO>(findUser);
//            responseDTO.Token = tokenString;

//            return Ok(new ApiResponse<UserLoginResponseDTO>(200, responseDTO, "User logged in successfully"));
//        }

//    }
//}






















////Id: 2
//// Code with Database, DTO, JWT, authorization, Cokkies


//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using WebApplication3.Data;
//using WebApplication3.DTO;
//using WebApplication3.Model;
//using WebApplication3.Utils;

//namespace WebApplication3.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly MobileDbContext _db;
//        private readonly IMapper _mapper;
//        private readonly IConfiguration _config;

//        public AuthController(MobileDbContext db, IMapper mapper, IConfiguration config)
//        {
//            _db = db;
//            _mapper = mapper;
//            _config = config;
//        }

//        [HttpPost("SignUp")]
//        public async Task<IActionResult> CreateUser([FromBody] UserModel userModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest("Validation Failed");
//            }

//            var findUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);
//            if (findUser != null)
//            {
//                return BadRequest($"User with {userModel.Email} Email ID already exists");
//            }

//            userModel.Id = (await _db.Users.MaxAsync(u => u.Id)) + 1;
//            userModel.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userModel.PasswordHash, 13);

//            await _db.Users.AddAsync(userModel);
//            await _db.SaveChangesAsync();

//            UserResponseDTO responseDTO = _mapper.Map<UserResponseDTO>(userModel);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "User Created Successfully"));
//        }

//        [HttpPost("Login")]
//        public async Task<IActionResult> Login([FromBody] UserLogin_DTO user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest("Validation failed");
//            }

//            var findUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
//            if (findUser == null)
//            {
//                return NotFound(new ApiError(404, "User Not Found"));
//            }

//            if (!BCrypt.Net.BCrypt.EnhancedVerify(user.PasswordHash, findUser.PasswordHash))
//            {
//                return BadRequest(new ApiError(400, "Invalid Password"));
//            }

//            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]);
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[]
//                {
//                    new Claim(ClaimTypes.Name, findUser.Id.ToString()),
//                    new Claim(ClaimTypes.Role, findUser.Role)
//                }),
//                Expires = DateTime.UtcNow.AddHours(1),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            var tokenString = tokenHandler.WriteToken(token);

//            // Store token in HttpOnly cookie
//            Response.Cookies.Append("AccessToken", tokenString, new CookieOptions
//            {
//                HttpOnly = true,                            // Prevent JavaScript access
//                Secure = true,                              // Send only over HTTPS (Set to false for development)
//                SameSite = SameSiteMode.Strict,             // Prevent CSRF
//                Expires = DateTime.UtcNow.AddMinutes(10)    // Token Expiry Time
//            });

//            var responseDTO = _mapper.Map<UserResponseDTO>(findUser);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "Login successful, token set in cookies"));
//        }
//    }
//}
























//Id: 3
// Code with Database, DTO, JWT, authorization, Cokkies -> Repository & Service 


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

