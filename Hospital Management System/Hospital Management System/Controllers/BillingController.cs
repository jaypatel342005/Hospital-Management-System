using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hospital_Management_System.Controllers
{
    public class BillingController : Controller
    {
        private IConfiguration _configuration;
        public BillingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_Billing_GetAll";
            connection.Open();
            using var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult RecordPayment(int BillID, decimal PaymentAmount)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_Billing_RecordPayment";
                    command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                    command.Parameters.Add("@PaymentAmount", SqlDbType.Decimal).Value = PaymentAmount;
                    command.ExecuteNonQuery();
                }
                TempData["SuccessMessage"] = "Payment recorded successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while recording the payment. Please try again or contact support.";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("Index");
        }

        public IActionResult GetBillingByAppointment(int AppointmentID)
        {
            var billingList = new List<BillingModel>();
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_Billing_GetByAppointment";
            command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                billingList.Add(new BillingModel
                {
                    BillID = reader.GetInt32("BillID"),
                    AppointmentID = reader.GetInt32("AppointmentID"),
                    BillAmount = reader.GetDecimal("BillAmount"),
                    PaidAmount = reader.GetDecimal("PaidAmount"),
                    PaymentStatus = reader.GetString("PaymentStatus"),
                    CreatedDate = reader.GetDateTime("CreatedDate")
                });
            }

            return View(billingList);
        }


        // NEW: Get billing records by Patient ID
        public IActionResult GetBillingByPatient(int PatientID)
        {
            var billingList = new List<EnhancedBillingModel>();
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_Billing_GetByPatientID";
            command.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                billingList.Add(new EnhancedBillingModel
                {
                    BillID = reader.GetInt32("BillID"),
                    AppointmentID = reader.GetInt32("AppointmentID"),
                    PatientID = reader.GetInt32("PatientID"),
                    PatientName = reader.GetString("PatientName"),
                    DoctorName = reader.GetString("DoctorName"),
                    AppointmentDate = reader.GetDateTime("AppointmentDate"),
                    AppointmentDescription = reader.GetString("AppointmentDescription"),
                    BillAmount = reader.GetDecimal("BillAmount"),
                    PaidAmount = reader.GetDecimal("PaidAmount"),
                    PaymentStatus = reader.GetString("PaymentStatus"),
                    CreatedDate = reader.GetDateTime("CreatedDate")
                });
            }

            return View(billingList);
        }






        public IActionResult GetOutstandingBills()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_Billing_GetOutstanding";
            connection.Open();
            using var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult Details(int BillID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_Billing_GetByID";
            command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
            connection.Open();
            using var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return View(table);
        }
    }
}