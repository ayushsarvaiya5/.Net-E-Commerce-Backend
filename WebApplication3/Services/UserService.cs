using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApplication3.DTO;
using WebApplication3.Interfaces;
using WebApplication3.Model;
using WebApplication3.Utils;

namespace WebApplication3.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserResponseDTO>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<List<UserResponseDTO>>(users);
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUSerByEmailAsync(email);
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<UserResponseDTO> UpdateUserAsync(UserUpdate_DTO userModel)
        {
            if (userModel.Id <= 0)
                throw new ArgumentException("Enter a valid Id");

            var findUser = await _userRepository.GetUserByIdAsync(userModel.Id ?? 0);
            if (findUser == null)
                throw new KeyNotFoundException("User not found.");

            var existingUser = await _userRepository.GetUserByEmailOrPhoneAsync(userModel.Email, userModel.PhoneNumber);
            if (existingUser != null && existingUser.Id != userModel.Id)
                throw new InvalidOperationException("Email or Phone number is already in use.");

            findUser.FirstName = userModel.FirstName ?? findUser.FirstName;
            findUser.LastName = userModel.LastName ?? findUser.LastName;
            findUser.Email = userModel.Email ?? findUser.Email;
            findUser.PhoneNumber = userModel.PhoneNumber ?? findUser.PhoneNumber;

            if (!string.IsNullOrEmpty(userModel.Password))
            {
                findUser.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userModel.Password, 13);
            }

            await _userRepository.UpdateUserAsync(findUser);
            return _mapper.Map<UserResponseDTO>(findUser);
        }

        public async Task<UserResponseDTO> ChangeUserRoleAsync(ChangeUserRole_DTO request)
        {
            var user = await _userRepository.GetUserByIdAsync(request.Id ?? 0);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.Role = request.NewRole;
            await _userRepository.UpdateUserAsync(user);
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            await _userRepository.DeleteUserAsync(user);
        }

        public async Task<List<UserResponseDTO>> GetUsersByRoleAsync(string role)
        {
            List<string> validRoles = new List<string> { "Customer", "Employee", "Admin" };
            if (!validRoles.Contains(role))
                throw new ArgumentException("Invalid Role. Role must be either 'Customer', 'Admin', or 'Employee'.");

            var users = await _userRepository.GetUsersByRoleAsync(role);
            return _mapper.Map<List<UserResponseDTO>>(users);
        }

        public async Task<(List<UserResponseDTO>, int)> GetUsersPaginatedAsync(int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Invalid Page or PageSize.");

            var (users, totalUsers) = await _userRepository.GetUsersPaginatedAsync(page, pageSize);
            var usersDTO = _mapper.Map<List<UserResponseDTO>>(users);

            return (usersDTO, totalUsers);
        }

    }
}
