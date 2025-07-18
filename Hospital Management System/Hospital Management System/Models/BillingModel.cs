namespace Hospital_Management_System.Models
{
    public class BillingModel
    {
        public int BillID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public decimal Amount { get; set; }
        public string Details { get; set; }
        public DateTime BillingDate { get; set; }
        public string Status { get; set; }
    }
}






