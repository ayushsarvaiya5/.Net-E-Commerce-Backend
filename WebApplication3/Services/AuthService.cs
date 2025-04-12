using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplication3.DTO;
using WebApplication3.Interfaces;
using WebApplication3.Model;
using WebApplication3.Utils;

namespace WebApplication3.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IConfiguration config, IMapper mapper)
        {
            _userRepository = userRepository;
            _config = config;
            _mapper = mapper;
        }

        public async Task<UserResponseDTO> SignUpService(UserModel userModel)
        {
            var findUser = await _userRepository.GetUSerByEmailAsync(userModel.Email);
            if (findUser != null)
            {
                throw new ArgumentException("User with given Email ID is already exists");
            }

            //userModel.Id = (await _userRepository.GetAllUsersAsync()).Max(u => u.Id) + 1;
            userModel.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userModel.PasswordHash, 13);

            await _userRepository.AddUserAsync(userModel);

            return _mapper.Map<UserResponseDTO>(userModel);
        }

        public async Task<(UserResponseDTO, string)> LoginService(UserLogin_DTO user)
        {
            var authenticatedUser = await AuthenticateUserAsync(user.Email, user.PasswordHash);
            if (authenticatedUser == null)
            {
                throw new InvalidOperationException("Invalid Email or Password");
            }

            var tokenString = GenerateJwtToken(authenticatedUser);

            var responseDTO = _mapper.Map<UserResponseDTO>(authenticatedUser);

            return (responseDTO, tokenString);
        }

        public async Task<UserModel?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUSerByEmailAsync(email);
            if (user != null && BCrypt.Net.BCrypt.EnhancedVerify(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        public string GenerateJwtToken(UserModel user)
        {
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
