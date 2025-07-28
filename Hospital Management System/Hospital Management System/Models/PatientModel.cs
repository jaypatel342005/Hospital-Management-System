using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management_System.Models
{
    public class PatientModel
    {
        public int? PatientID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(10, ErrorMessage = "Gender cannot be longer than 10 characters")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"[1-9]{10}$", ErrorMessage = "Please enter a valid phone number")]
        [StringLength(100, ErrorMessage = "Phone cannot be longer than 100 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(250, ErrorMessage = "Address cannot be longer than 250 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City cannot be longer than 100 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(100, ErrorMessage = "State cannot be longer than 100 characters")]
        public string State { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid user")]
        public int UserID { get; set; }

        // Navigation Property (if you have a User model)
        //[ForeignKey("UserID")]
        //public virtual User? User { get; set; }
    }
}