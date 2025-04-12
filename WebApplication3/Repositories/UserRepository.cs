using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Interfaces;
using WebApplication3.Model;

namespace WebApplication3.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MobileDbContext _db;

        public UserRepository(MobileDbContext db) 
        {
            _db = db;
        }

        public async Task AddUserAsync(UserModel user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<UserModel?> GetUSerByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task UpdateUserAsync(UserModel user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<UserModel?> GetUserByEmailOrPhoneAsync(string? email, string? phoneNumber)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email || u.PhoneNumber == phoneNumber);
        }

        public async Task DeleteUserAsync(UserModel user)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task<List<UserModel>> GetUsersByRoleAsync(string role)
        {
            return await _db.Users.Where(u => u.Role == role).ToListAsync();
        }

        public async Task<(List<UserModel>, int)> GetUsersPaginatedAsync(int page, int pageSize)
        {
            var totalUsers = await _db.Users.CountAsync();

            var users = await _db.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalUsers);
        }

    }
}
