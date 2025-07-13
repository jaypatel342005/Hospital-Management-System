using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management_System.Models
{
    public class PatientModel
    {
        [Key]
        public int PatientID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string State { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; }

        // Foreign Key
        [Required]
        public int UserID { get; set; }

        //// Navigation Property (if you have a User model)
        //[ForeignKey("UserID")]
        //public virtual User User { get; set; }
    }
}
