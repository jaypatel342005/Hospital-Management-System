using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class DoctorDepartmentsModel
    {
        public int? DoctorDepartmentID { get; set; }

        [Required(ErrorMessage = "Doctor is required")]
        [Display(Name = "Doctor")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "User is required")]
        [Display(Name = "User")]
        public int UserID { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

        // Additional properties for display purposes (from joins)
        public string? DoctorName { get; set; }
        public string? DepartmentName { get; set; }
        public string? UserName { get; set; }
    }
}