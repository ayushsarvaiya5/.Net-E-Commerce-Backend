using WebApplication3.DTO;

namespace WebApplication3.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserResponseDTO>> GetUsersAsync();

        public Task<UserResponseDTO> GetUserByIdAsync(int id);

        public Task<UserResponseDTO> GetUserByEmailAsync(string email);

        public Task<UserResponseDTO> UpdateUserAsync(UserUpdate_DTO userModel);

        public Task<UserResponseDTO> ChangeUserRoleAsync(ChangeUserRole_DTO request);

        public Task DeleteUserAsync(int id);

        public Task<List<UserResponseDTO>> GetUsersByRoleAsync(string role);

        public Task<(List<UserResponseDTO>, int)> GetUsersPaginatedAsync(int page, int pageSize);
    }
}
