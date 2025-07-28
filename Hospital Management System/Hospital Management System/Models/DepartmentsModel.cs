using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management_System.Models
{
    public class DepartmentsModel
    {
        [Key]
        public int? DepartmentID { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }


        public DateTime? Created { get; set; }

     
        public DateTime? Modified { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}
