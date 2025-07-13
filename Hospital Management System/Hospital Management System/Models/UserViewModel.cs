using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ProfilePicture { get; set; }
        
        public DateTime? CreatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
    }
}