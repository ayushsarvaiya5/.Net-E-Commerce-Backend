using WebApplication3.DTO;
using WebApplication3.Model;

namespace WebApplication3.Interfaces
{
    public interface IAuthService
    {
        public Task<UserResponseDTO> SignUpService(UserModel userModel);

        public Task<(UserResponseDTO, string)> LoginService(UserLogin_DTO user);

        public Task<UserModel?> AuthenticateUserAsync(string email, string password);

        public string GenerateJwtToken(UserModel user);
    }
}
