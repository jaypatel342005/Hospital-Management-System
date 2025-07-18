using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Hospital_Management_System.Models
{
    public class AppointmentModel
    {
        public int AppointmentID { get; set; }

        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public int UserID { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string AppointmentStatus { get; set; }
        public string Description { get; set; }
        public string SpecialRemarks { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public decimal? TotalConsultedAmount { get; set; }

        // Joined fields
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string UserName { get; set; }

    }
}
