//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using WebApplication3.Model;
//using WebApplication3.DTO;
//using WebApplication3.Data;

//namespace WebApplication3.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {

//        private readonly MobileDbContext db;

//        public UserController(MobileDbContext db)
//        {
//            this.db = db;
//        }

//        //public List<UserModel> Users = new List<UserModel>()
//        //{
//        //    new UserModel(1, "a", "b", "a@gmail.com", "password", "9898366148", "User"),
//        //    new UserModel(2, "a", "b", "a@gmail.com", "password", "9898366148", "User"),
//        //    new UserModel(3, "a", "b", "a@gmail.com", "password", "9898366148", "User"),
//        //    new UserModel(4, "a", "b", "a@gmail.com", "password", "9898366148", "User"),
//        //    new UserModel(5, "a", "b", "a@gmail.com", "password", "9898366148", "User"),
//        //    new UserModel(6, "a", "b", "a@gmail.com", "password", "9898366148", "User"),
//        //    new UserModel(7, "a", "b", "a@gmail.com", "password", "9898366148", "User"),
//        //    new UserModel(8, "a", "b", "a@gmail.com", "password", "9898366148", "User"),
//        //    new UserModel(9, "a", "b", "a@gmail.com", "password", "9898366148", "User"),
//        //};

//        [HttpPost("SignUp")]
//        public IActionResult CreateUser([FromBody] UserModel userModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var findUser = db.Users.FirstOrDefault(u => u.Email == userModel.Email);

//            if (findUser != null)
//            {
//                return BadRequest($"User with {userModel.Email} Email ID already exist");
//            }

//            userModel.Id = Users.Count + 1;

//            Users.Add(userModel);
//            return Ok(Users);
//        }

//        [HttpPost("Login")]
//        public ActionResult<UserModel> login([FromBody] UserLogin_DTO user)
//        {
//            if (!ModelState.IsValid) {
//                return BadRequest(ModelState);
//            }

//            var findUser = Users.Find(u =>  u.Email == user.Email);

//            if (findUser == null)
//            {
//                return NotFound("User Not Found");
//            }

//            if(user.PasswordHash != findUser.PasswordHash)
//            {
//                return BadRequest("Invalid Password");
//            }

//            return Ok("User login sucessfully");
//        }

//        [HttpGet("Get-All-Users")]
//        public IActionResult GetAllUsers()
//        {
//            return Ok(Users);
//        }

//        [HttpGet("Get-UserById/{id}")]
//        public IActionResult GetUserById(int id)
//        {
//            var findUser = Users.Find(u => u.Id == id);

//            if (findUser == null)
//            {
//                return NotFound("User Not Found");
//            }

//            return Ok(findUser);
//        }

//        [HttpGet("Search-UserByEmail/{email}")]
//        public IActionResult SearchUserByEmail(String email)
//        {
//            if (String.IsNullOrEmpty(email))
//            {
//                return BadRequest("Email is REquired");
//            }

//            var findUser = Users.Find(u => u.Email == email);

//            if (findUser == null)
//            {
//                return NotFound("User Not Found With given Email");
//            }

//            return Ok(findUser);
//        }

//        [HttpPatch("Update-User")]
//        public IActionResult UpdateUser([FromBody] UserUpdate_DTO userModel)
//        {

//            if (userModel.Id <= 0)
//            {
//                return BadRequest("Id is Required");
//            }

//            var findUser = Users.Find(u => u.Id == userModel.Id);

//            if (findUser == null)
//            {
//                return NotFound("User not found.");
//            }

//            // Update only non-null properties
//            if (!string.IsNullOrEmpty(userModel.FirstName))
//            {
//                findUser.FirstName = userModel.FirstName;
//            }

//            if (!string.IsNullOrEmpty(userModel.LastName))
//            {
//                findUser.LastName = userModel.LastName;
//            }

//            if (!string.IsNullOrEmpty(userModel.Email))
//            {
//                findUser.Email = userModel.Email;
//            }

//            if (!string.IsNullOrEmpty(userModel.PasswordHash))
//            {
//                findUser.PasswordHash = userModel.PasswordHash;
//            }

//            if (!string.IsNullOrEmpty(userModel.PhoneNumber))
//            {
//                findUser.PhoneNumber = userModel.PhoneNumber;
//            }

//            return Ok(findUser);
//        }

//        [HttpPatch("Change-UserRole/{id}")]
//        public IActionResult ChangeUserRole(int id, [FromBody] string newRole)
//        {
//            var validRoles = new List<string> { "User", "Admin", "Employee" };

//            if (!validRoles.Contains(newRole))
//            {
//                return BadRequest("Invalid role. Allowed values: User, Admin, Employee.");
//            }

//            var findUser = Users.Find(u => u.Id == id);

//            if (findUser == null)
//            {
//                return NotFound("User not found");
//            }

//            findUser.Role = newRole;

//            return Ok(findUser);
//        }

//        [HttpDelete("Delete-USer/{id}")]
//        public IActionResult DeleteUser(int id)
//        {
//            var user = Users.Find(u => u.Id == id);

//            if (user == null)
//            {
//                return NotFound("User not found.");
//            }

//            Users.Remove(user);

//            return Ok(new { message = "User deleted successfully." });
//        }

//        [HttpGet("Get-Users-Paginated")]
//        public IActionResult GetUsersPaginated(int page = 1, int pageSize = 2)
//        {
//            var paginatedUsers = Users
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToList();

//            return Ok(new
//            {
//                Page = page,
//                PageSize = pageSize,
//                TotalUsers = Users.Count,
//                Users = paginatedUsers
//            });
//        }

//    }
//}














// ID : 1
// Code with Database, DTO, JWT NORMAL 



//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication3.Model;
//using WebApplication3.DTO;
//using WebApplication3.Data;
//using WebApplication3.Utils;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using AutoMapper;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.AspNetCore.Authorization;
//using System.Numerics;

//namespace WebApplication3.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private readonly MobileDbContext _db;
//        private readonly IMapper _mapper;

//        public UserController(MobileDbContext db, IMapper mapper)
//        {
//            _db = db;
//            _mapper = mapper;
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [HttpGet("Get-All-Users")]
//        public async Task<IActionResult> GetAllUsers()
//        {
//            //var users = await _db.Users
//            //    .Select(u => new UserResponseDTO    // Converting UserModel to UserResponseDTO -> For Hididing Password
//            //    {
//            //        Id = u.Id,
//            //        FirstName = u.FirstName,
//            //        LastName = u.LastName,
//            //        Email = u.Email,
//            //        PhoneNumber = u.PhoneNumber,
//            //        Role = u.Role
//            //    })
//            //    .ToListAsync();

//            var users = await _db.Users.ToListAsync();

//            var responseDTO = _mapper.Map<List<UserResponseDTO>>(users);

//            return Ok(new ApiResponse<List<UserResponseDTO>>(200, responseDTO, "Fetching Successfull"));
//        }

//        [Authorize]
//        [HttpGet("Get-CurrentUser/{id:int}")]
//        public async Task<IActionResult> GetUserById(int id)
//        {
//            var loggedInUserId = User.FindFirstValue(ClaimTypes.Name);
//            var loggedInUserRole = User.FindFirstValue(ClaimTypes.Role);

//            if (String.IsNullOrEmpty(loggedInUserId))
//            {
//                return Unauthorized(new ApiError(401, "Invalid token."));
//            }

//            if ((loggedInUserId != id.ToString()) && (loggedInUserRole != "Admin"))
//            {
//                return Forbid();
//            }

//            var findUser = await _db.Users.FindAsync(id);
//            if (findUser == null)
//            {
//                return NotFound(new ApiError(404, "User Not Found"));
//            }

//            //UserResponseDTO newUser = new UserResponseDTO
//            //{
//            //    Id = findUser.Id,
//            //    FirstName = findUser.FirstName,
//            //    LastName = findUser.LastName,
//            //    Email = findUser.Email,
//            //    PhoneNumber = findUser.PhoneNumber,
//            //    Role = findUser.Role
//            //};

//            var responseDTO = _mapper.Map<UserResponseDTO>(findUser);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "Fetching Successfully"));
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [HttpGet("Get-UserByEmail/{email}")]
//        public async Task<IActionResult> GetUserByEmail(string email)
//        {
//            if (string.IsNullOrEmpty(email))
//            {
//                return BadRequest(new ApiError(400, "Email is required"));
//            }

//            var findUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
//            if (findUser == null)
//            {
//                return NotFound(new ApiError(404, "User Not Found With given Email"));
//            }

//            //UserResponseDTO newUser = new UserResponseDTO
//            //{
//            //    Id = findUser.Id,
//            //    FirstName = findUser.FirstName,
//            //    LastName = findUser.LastName,
//            //    Email = findUser.Email,
//            //    PhoneNumber = findUser.PhoneNumber,
//            //    Role = findUser.Role
//            //};

//            var responseDTO = _mapper.Map<UserResponseDTO>(findUser);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "Fetch Successfully"));
//        }

//        [Authorize]
//        [HttpPatch("Update-User")]
//        public async Task<IActionResult> UpdateUser([FromBody] UserUpdate_DTO userModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest("Validation Failed");
//            }

//            if (userModel.Id <= 0)
//            {
//                return BadRequest(new ApiError(400, "Enter a valid Id"));
//            }

//            // Get the logged-in user's ID and Role from the JWT token
//            var loggedInUserId = User.FindFirst(ClaimTypes.Name)?.Value; // Extract user ID from token
//            var loggedInUserRole = User.FindFirst(ClaimTypes.Role)?.Value; // Extract user Role from token

//            if (string.IsNullOrEmpty(loggedInUserId))
//            {
//                return Unauthorized(new ApiError(401, "Invalid token."));
//            }

//            // Ensure user can only update their own profile or is an Admin
//            if ((loggedInUserRole != "Admin") && (loggedInUserId != userModel.Id.ToString()))
//            {
//                return Forbid(); // User is not the owner of the profile
//            }

//            var findUser = await _db.Users.FindAsync(userModel.Id);
//            if (findUser == null)
//            {
//                return NotFound(new ApiError(404, "User not found."));
//            }

//            var existingUser = await _db.Users
//                .Where(u => (u.Email == userModel.Email || u.PhoneNumber == userModel.PhoneNumber))
//                .FirstOrDefaultAsync();

//            if (existingUser != null)
//            {
//                if (existingUser.Email == userModel.Email)
//                {
//                    return BadRequest(new ApiError(400, $"{userModel.Email} Email is in use. Enter a unique email."));
//                }
//                if (existingUser.PhoneNumber == userModel.PhoneNumber)
//                {
//                    return BadRequest(new ApiError(400, $"{userModel.PhoneNumber} Phone number is in use. Enter a unique phone number."));
//                }
//            }

//            if (!string.IsNullOrEmpty(userModel.FirstName))
//            {
//                findUser.FirstName = userModel.FirstName;
//            }

//            if (!string.IsNullOrEmpty(userModel.LastName))
//            {
//                findUser.LastName = userModel.LastName;
//            }

//            if (!string.IsNullOrEmpty(userModel.Email))
//            {
//                findUser.Email = userModel.Email;
//            }

//            if (!string.IsNullOrEmpty(userModel.Password))
//            {
//                findUser.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userModel.Password, 13);
//            }

//            if (!string.IsNullOrEmpty(userModel.PhoneNumber))
//            {
//                findUser.PhoneNumber = userModel.PhoneNumber;
//            }

//            _db.Users.Update(findUser);
//            await _db.SaveChangesAsync();

//            var responseDTO = _mapper.Map<UserResponseDTO>(findUser);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "User Successfully Updated"));
//        }

//        [Authorize(Policy = "AdminOnly")]
//        [HttpPatch("Change-UserRole")]
//        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRole_DTO request)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(new ApiError(400, "Validation Failed"));
//            }

//            var findUser = await _db.Users.FindAsync(request.Id);
//            if (findUser == null)
//            {
//                return NotFound(new ApiError(404, "User not found"));
//            }

//            findUser.Role = request.NewRole;
//            _db.Users.Update(findUser);
//            await _db.SaveChangesAsync();

//            //UserResponseDTO newUser = new UserResponseDTO
//            //{
//            //    Id = findUser.Id,
//            //    FirstName = findUser.FirstName,
//            //    LastName = findUser.LastName,
//            //    Email = findUser.Email,
//            //    PhoneNumber = findUser.PhoneNumber,
//            //    Role = findUser.Role
//            //};

//            var responseDTO = _mapper.Map<UserResponseDTO>(findUser);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "Roll updte successfully"));
//        }

//        [Authorize(Policy = "AdminOnly")]
//        [HttpDelete("Delete-User/{id}")]
//        public async Task<IActionResult> DeleteUser(int id)
//        {
//            var user = await _db.Users.FindAsync(id);
//            if (user == null)
//            {
//                return NotFound(new ApiError(404, "User not found"));
//            }

//            _db.Users.Remove(user);
//            await _db.SaveChangesAsync();

//            return Ok(new ApiResponse<string>(200, "User deleted successfully"));
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet("Get-Users-ByRole/{role:alpha}")]
//        public async Task<IActionResult> GetUserByRole(string role)
//        {
//            List<String> validRoles = new List<string> { "Customer", "Employee", "Admin" };

//            if (string.IsNullOrEmpty(role))
//            {
//                return BadRequest(new ApiError(400, "Role is required"));
//            }

//            if (!validRoles.Contains(role))
//            {
//                return BadRequest(new ApiError(400, "Invalid Role. Role must be either 'Customer', 'Admin', or 'Employee'."));
//            }

//            //var findUsers = await _db.Users.Where(u => u.Role == role)
//            //    .Select(u => new UserResponseDTO
//            //    {
//            //        Id = u.Id,
//            //        FirstName = u.FirstName,
//            //        LastName = u.LastName,
//            //        Email = u.Email,
//            //        PhoneNumber = u.PhoneNumber,
//            //        Role = u.Role
//            //    })
//            //    .ToListAsync();

//            var findUsers = await _db.Users.Where(u => u.Role == role).ToListAsync();

//            var responseDTO = _mapper.Map<List<UserResponseDTO>>(findUsers);

//            return Ok(new ApiResponse<List<UserResponseDTO>>(200, responseDTO, "Fetching sucessfull"));
//        }

//        [Authorize(Roles = "Admin,Employee")]
//        [HttpGet("Get-Users-Paginated")]
//        public async Task<IActionResult> GetUsersPaginated(int page = 1, int pageSize = 2)
//        {
//            if (page < 1 || pageSize < 1)
//            {
//                return BadRequest(new ApiError(400, "Invalid Page or Pagesize"));
//            }

//            var totalUsers = await _db.Users.CountAsync();

//            //var paginatedUsers = await _db.Users
//            //    .Skip((page - 1) * pageSize)
//            //    .Take(pageSize)
//            //    .Select(u =>
//            //    new UserResponseDTO
//            //    {
//            //        Id = u.Id,
//            //        FirstName = u.FirstName,
//            //        LastName = u.LastName,
//            //        Email = u.Email,
//            //        PhoneNumber = u.PhoneNumber,
//            //        Role = u.Role
//            //    })
//            //    .ToListAsync();

//            var paginatedUsers = await _db.Users
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToListAsync();

//            if (!paginatedUsers.Any())
//            {
//                return NotFound(new ApiError(404, "No users found for the given page"));
//            }

//            var responseDTO = _mapper.Map<List<UserResponseDTO>>(paginatedUsers);

//            return Ok(new ApiResponse<object>(
//                200,
//                new
//                {
//                    Page = page,
//                    PageSize = pageSize,
//                    TotalUsers = totalUsers,
//                    Users = responseDTO
//                },
//                "Pagination Successfull"
//                ));
//        }
//    }
//}

















//// Id: 2
//// Code with Database, DTO, JWT, authorization, Cokkies


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication3.Model;
//using WebApplication3.DTO;
//using WebApplication3.Data;
//using WebApplication3.Utils;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using AutoMapper;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.AspNetCore.Authorization;
//using System.Numerics;

//namespace WebApplication3.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private readonly MobileDbContext _db;
//        private readonly IMapper _mapper;

//        public UserController(MobileDbContext db, IMapper mapper)
//        {
//            _db = db;
//            _mapper = mapper;
//        }

//        [Authorize]
//        [HttpGet("GetUser")]
//        public IActionResult GetUser()
//        {
//            var user = HttpContext.Items["User"] as UserResponseDTO;
//            if (user == null)
//            {
//                return Unauthorized(new { message = "Invalid token" });
//            }

//            return Ok(new ApiResponse<UserResponseDTO>(200, user));
//        }


//        [Authorize(Roles = "Admin, Employee")]
//        [HttpGet("Get-All-Users")]
//        public async Task<IActionResult> GetAllUsers()
//        {
//            var users = await _db.Users.ToListAsync();

//            var responseDTO = _mapper.Map<List<UserResponseDTO>>(users);

//            return Ok(new ApiResponse<List<UserResponseDTO>>(200, responseDTO, "Fetching Successfull"));
//        }

//        [Authorize]
//        [HttpGet("Get-User-ById/{id:int}")]
//        public async Task<IActionResult> GetUserById(int id)
//        {
//            var loggedInUserId = User.FindFirstValue(ClaimTypes.Name);
//            var loggedInUserRole = User.FindFirstValue(ClaimTypes.Role);

//            if (String.IsNullOrEmpty(loggedInUserId))
//            {
//                return Unauthorized(new ApiError(401, "Invalid token."));
//            }

//            if ((loggedInUserId != id.ToString()) && (loggedInUserRole != "Admin"))
//            {
//                return Forbid();
//            }

//            var findUser = await _db.Users.FindAsync(id);
//            if (findUser == null)
//            {
//                return NotFound(new ApiError(404, "User Not Found"));
//            }

//            var responseDTO = _mapper.Map<UserResponseDTO>(findUser);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "Fetching Successfully"));
//        }

//        [Authorize(Roles = "Admin, Employee")]
//        [HttpGet("Get-UserByEmail/{email}")]
//        public async Task<IActionResult> GetUserByEmail(string email)
//        {
//            if (string.IsNullOrEmpty(email))
//            {
//                return BadRequest(new ApiError(400, "Email is required"));
//            }

//            var findUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
//            if (findUser == null)
//            {
//                return NotFound(new ApiError(404, "User Not Found With given Email"));
//            }

//            var responseDTO = _mapper.Map<UserResponseDTO>(findUser);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "Fetch Successfully"));
//        }

//        [Authorize]
//        [HttpPatch("Update-User")]
//        public async Task<IActionResult> UpdateUser([FromBody] UserUpdate_DTO userModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest("Validation Failed");
//            }

//            if (userModel.Id <= 0)
//            {
//                return BadRequest(new ApiError(400, "Enter a valid Id"));
//            }

//            var loggedInUser = HttpContext.Items["User"] as UserResponseDTO;
//            int loggedInUserId = loggedInUser.Id;
//            string loggedInUserRole = loggedInUser.Role;

//            if ((loggedInUserRole != "Admin") && (loggedInUserId != userModel.Id))
//            {
//                return Forbid();
//            }

//            var findUser = await _db.Users.FindAsync(userModel.Id);
//            if (findUser == null)
//            {
//                return NotFound(new ApiError(404, "User not found."));
//            }

//            var existingUser = await _db.Users
//                .Where(u => (u.Email == userModel.Email || u.PhoneNumber == userModel.PhoneNumber))
//                .FirstOrDefaultAsync();

//            if (existingUser != null)
//            {
//                if (existingUser.Email == userModel.Email)
//                {
//                    return BadRequest(new ApiError(400, $"{userModel.Email} Email is in use. Enter a unique email."));
//                }
//                if (existingUser.PhoneNumber == userModel.PhoneNumber)
//                {
//                    return BadRequest(new ApiError(400, $"{userModel.PhoneNumber} Phone number is in use. Enter a unique phone number."));
//                }
//            }

//            if (!string.IsNullOrEmpty(userModel.FirstName))
//            {
//                findUser.FirstName = userModel.FirstName;
//            }

//            if (!string.IsNullOrEmpty(userModel.LastName))
//            {
//                findUser.LastName = userModel.LastName;
//            }

//            if (!string.IsNullOrEmpty(userModel.Email))
//            {
//                findUser.Email = userModel.Email;
//            }

//            if (!string.IsNullOrEmpty(userModel.Password))
//            {
//                findUser.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userModel.Password, 13);
//            }

//            if (!string.IsNullOrEmpty(userModel.PhoneNumber))
//            {
//                findUser.PhoneNumber = userModel.PhoneNumber;
//            }

//            _db.Users.Update(findUser);
//            await _db.SaveChangesAsync();

//            var responseDTO = _mapper.Map<UserResponseDTO>(findUser);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "User Successfully Updated"));
//        }

//        [Authorize(Policy = "AdminOnly")]
//        [HttpPatch("Change-UserRole")]
//        public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRole_DTO request)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(new ApiError(400, "Validation Failed"));
//            }

//            var findUser = await _db.Users.FindAsync(request.Id);
//            if (findUser == null)
//            {
//                return NotFound(new ApiError(404, "User not found"));
//            }

//            findUser.Role = request.NewRole;
//            _db.Users.Update(findUser);
//            await _db.SaveChangesAsync();

//            var responseDTO = _mapper.Map<UserResponseDTO>(findUser);

//            return Ok(new ApiResponse<UserResponseDTO>(200, responseDTO, "Roll updte successfully"));
//        }

//        [Authorize(Policy = "AdminOnly")]
//        [HttpDelete("Delete-User/{id}")]
//        public async Task<IActionResult> DeleteUser(int id)
//        {
//            var user = await _db.Users.FindAsync(id);
//            if (user == null)
//            {
//                return NotFound(new ApiError(404, "User not found"));
//            }

//            _db.Users.Remove(user);
//            await _db.SaveChangesAsync();

//            return Ok(new ApiResponse<string>(200, "User deleted successfully"));
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet("Get-Users-ByRole/{role:alpha}")]
//        public async Task<IActionResult> GetUserByRole(string role)
//        {
//            List<String> validRoles = new List<string> { "Customer", "Employee", "Admin" };

//            if (string.IsNullOrEmpty(role))
//            {
//                return BadRequest(new ApiError(400, "Role is required"));
//            }

//            if (!validRoles.Contains(role))
//            {
//                return BadRequest(new ApiError(400, "Invalid Role. Role must be either 'Customer', 'Admin', or 'Employee'."));
//            }

//            var findUsers = await _db.Users.Where(u => u.Role == role).ToListAsync();

//            var responseDTO = _mapper.Map<List<UserResponseDTO>>(findUsers);

//            return Ok(new ApiResponse<List<UserResponseDTO>>(200, responseDTO, "Fetching sucessfull"));
//        }

//        [Authorize(Roles = "Admin,Employee")]
//        [HttpGet("Get-Users-Paginated")]
//        public async Task<IActionResult> GetUsersPaginated(int page = 1, int pageSize = 2)
//        {
//            if (page < 1 || pageSize < 1)
//            {
//                return BadRequest(new ApiError(400, "Invalid Page or Pagesize"));
//            }

//            var totalUsers = await _db.Users.CountAsync();

//            var paginatedUsers = await _db.Users
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToListAsync();

//            if (!paginatedUsers.Any())
//            {
//                return NotFound(new ApiError(404, "No users found for the given page"));
//            }

//            var responseDTO = _mapper.Map<List<UserResponseDTO>>(paginatedUsers);

//            return Ok(new ApiResponse<object>(
//                200,
//                new
//                {
//                    Page = page,
//                    PageSize = pageSize,
//                    TotalUsers = totalUsers,
//                    Users = responseDTO
//                },
//                "Pagination Successfull"
//                ));
//        }
//    }
//}




















// Id: 3
// Code with Database, DTO, JWT, authorization, Cokkies -> REpository & SErvices


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
    //[Route("api/User")]
    //[ApiController]
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


        //[Authorize(Roles = "Admin, Employee")]
        //[HttpGet("Get-All-Users")]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _userService.GetUsersAsync();

        //    // Set sunset headers
        //    Response.Headers["Deprecation"] = "Sun, 20 Apr 2025 00:00:00 GMT";
        //    Response.Headers["Link"] = "</api/v2/Get-All-Users>; rel=\"successor-version\"";
        //    Response.Headers["Warning"] = "299 - \"This API version will be removed after 20-04-2025. Use /api/v2/Get-All-Users instead.\"";

        //    return Ok(new ApiResponse<List<UserResponseDTO>>(200, users, "Fetching Successfull"));
        //}

        //[Authorize(Roles = "Admin, Employee")]
        //[HttpGet("Get-User-ById/{id:int}")]
        //public async Task<IActionResult> GetUserById(int id)
        //{
        //    var findUser = await _userService.GetUserByIdAsync(id);

        //    if (findUser == null)
        //    {
        //        return NotFound(new ApiError(404, "User Not Found"));
        //    }

        //    return Ok(new ApiResponse<UserResponseDTO>(200, findUser, "Fetching Successfully"));
        //}

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
