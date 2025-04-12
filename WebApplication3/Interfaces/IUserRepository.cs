using WebApplication3.Model;

namespace WebApplication3.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<UserModel>> GetAllUsersAsync();

        public Task<UserModel?> GetUserByIdAsync(int id);

        public Task<UserModel?> GetUSerByEmailAsync(string email);

        public Task AddUserAsync(UserModel user);

        public Task UpdateUserAsync(UserModel user);

        public Task<UserModel?> GetUserByEmailOrPhoneAsync(string? email, string? phoneNumber);

        public Task DeleteUserAsync(UserModel user);

        public Task<List<UserModel>> GetUsersByRoleAsync(string role);

        public Task<(List<UserModel>, int)> GetUsersPaginatedAsync(int page, int pageSize);
    }
}
