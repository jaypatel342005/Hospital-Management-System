using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management_System.Models
{
    public class AppointmentModel
    {
        public int? AppointmentID { get; set; }

        [Required(ErrorMessage = "Doctor is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid doctor")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Patient is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid patient")]
        public int PatientID { get; set; }

      
        public int UserID { get; set; }

        [Required(ErrorMessage = "Appointment date is required")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Appointment status is required")]
        [StringLength(50, ErrorMessage = "Appointment status cannot be longer than 50 characters")]
        public string AppointmentStatus { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }

        [StringLength(250, ErrorMessage = "Special remarks cannot be longer than 250 characters")]
        public string SpecialRemarks { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Consulted amount must be a positive value")]
        [DataType(DataType.Currency)]
        public decimal? TotalConsultedAmount { get; set; }

        // Joined fields - These are typically populated from joins and don't need validation
        [StringLength(100, ErrorMessage = "Doctor name cannot be longer than 100 characters")]
        public string? DoctorName { get; set; }

        [StringLength(100, ErrorMessage = "Patient name cannot be longer than 100 characters")]
        public string? PatientName { get; set; }

        [StringLength(100, ErrorMessage = "User name cannot be longer than 100 characters")]
        public string? UserName { get; set; }

        // Navigation Properties (if you have related models)
        //[ForeignKey("DoctorID")]
        //public virtual Doctor? Doctor { get; set; }

        //[ForeignKey("PatientID")]
        //public virtual Patient? Patient { get; set; }

        //[ForeignKey("UserID")]
        //public virtual User? User { get; set; }
    }
}