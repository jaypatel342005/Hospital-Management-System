using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management_System.Models
{
    public class DoctorModel
    {
        public int? DoctorID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"[1-9]{10}$", ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Qualification is required")]
        [StringLength(200, ErrorMessage = "Qualification cannot be longer than 200 characters")]
        public string Qualification { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(200, ErrorMessage = "Specialization cannot be longer than 200 characters")]
        public string Specialization { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

    
        public int UserID { get; set; }

        // Navigation property (assumes User model exists)
        //[ForeignKey("UserID")]
        //public virtual User? User { get; set; }
    }
}
