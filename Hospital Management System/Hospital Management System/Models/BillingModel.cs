namespace Hospital_Management_System.Models
{
    public class BillingModel
    {
        public int BillID { get; set; }
        public int AppointmentID { get; set; }
        public decimal BillAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        // Computed property for balance
        public decimal BalanceAmount => BillAmount - PaidAmount;
    }

    public class EnhancedBillingModel
    {
        public int BillID { get; set; }
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentDescription { get; set; }
        public decimal BillAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        // Computed properties
        public decimal BalanceAmount => BillAmount - PaidAmount;
        public string FormattedAppointmentDate => AppointmentDate.ToString("MMM dd, yyyy hh:mm tt");
        public string FormattedCreatedDate => CreatedDate.ToString("MMM dd, yyyy hh:mm tt");
        public string PaymentStatusColor => PaymentStatus switch
        {
            "Paid" => "success",
            "Unpaid" => "danger",
            "Partial" => "warning",
            _ => "secondary"
        };
    }






    public class PaymentRecordModel
    {
        public int BillID { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}