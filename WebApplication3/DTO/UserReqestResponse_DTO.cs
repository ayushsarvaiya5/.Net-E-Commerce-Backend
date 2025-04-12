using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTO
{
    // DTO of Users
    public class UserLoginResponseDTO
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }

        public string? Token { get; set; }
    }

    public class UserResponseDTO
    {
        [Required(ErrorMessage = "Id is Required")]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
    }

    public class UserV2ResponseDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
    }


    public class UserLogin_DTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Column("Password")]
        public string PasswordHash { get; set; }
    }

    public class UserUpdate_DTO
    {
        [Required]
        public int? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email is not valid.")]
        public string? Email { get; set; }

        public string? Password { get; set; }

        [RegularExpression(@"^(\+91[\-\s]?)?[6-9]\d{9}$", ErrorMessage = "Invalid Indian phone number")]
        public string? PhoneNumber { get; set; }

    }

    public class ChangeUserRole_DTO
    {
        [Required(ErrorMessage = "Id is required.")]
        public int? Id { get; set; } = null;

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression(@"^(Customer|Admin|Employee)$", ErrorMessage = "Role must be either 'Customer', 'Admin', or 'Employee'.")]
        public string? NewRole { get; set; } = null;
    }
}
