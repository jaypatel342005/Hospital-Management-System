using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management_System.Models
{



    public class UsersModel
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [StringLength(100, ErrorMessage = "User Name cannot exceed 100 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [StringLength(100, ErrorMessage = "Mobile Number cannot exceed 100 characters")]
        public string MobileNo { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

        [NotMapped]
        public IFormFile profileImg { get; set; }

    }


    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
    ErrorMessage = "Password must be at least 8 characters long and contain one uppercase, one lowercase, one number, and one special character.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")] // RFC 5321 limit
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [RegularExpression(@"^(\+\d{1,3}[- ]?)?\d{10,14}$",
            ErrorMessage = "Mobile number must be 10-14 digits and may include country code")]
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; } = true;
    }



    public class UserLoginModel
    {
        [Required(ErrorMessage = "Username or Email is required.")]
        public string LoginIdentifier { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }


}


