using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApplication3.Model
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UserId")]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; } = null;

        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; } = null;

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email is not valid.")]
        public string? Email { get; set; } = null;

        [Required(ErrorMessage = "Password is required")]
        [Column("Password")]
        public string? PasswordHash { get; set; } = null;

        [Required(ErrorMessage = "Phone no is required")]
        [RegularExpression(@"^(\+91[\-\s]?)?[6-9]\d{9}$", ErrorMessage = "Invalid Indian phone number")]
        public string? PhoneNumber { get; set; } = null;

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression(@"^(Customer|Admin|Employee)$", ErrorMessage = "Role must be either 'Customer', 'Admin', or 'Employee'.")]
        public string? Role { get; set; } = null;

    }
}
