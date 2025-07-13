using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management_System.Models
{
    public class DoctorModel
    {
        [Key]
        public int DoctorID { get; set; }

        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Qualification { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public DateTime Modified { get; set; }

        [Required]
        public int UserID { get; set; }

        // Navigation property (assumes User model exists)
        //[ForeignKey("UserID")]
        //public virtual User User { get; set; }
    }
}
