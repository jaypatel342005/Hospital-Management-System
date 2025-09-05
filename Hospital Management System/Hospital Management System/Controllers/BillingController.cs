using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

using static System.Runtime.InteropServices.JavaScript.JSType;
using IronPdf;

namespace Hospital_Management_System.Controllers
{
    [CheckAccess]
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

        [EncryptedActionParameter]
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

        [EncryptedActionParameter]
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
        [EncryptedActionParameter]
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

        [EncryptedActionParameter]
        public IActionResult Details(int BillID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            // Get Billing Details using existing stored procedure
            DataTable billingTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Billing_GetByID";
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                connection.Open();
                using var reader = command.ExecuteReader();
                billingTable = new DataTable();
                billingTable.Load(reader);
            }

            // Check if bill exists
            if (billingTable.Rows.Count == 0)
            {
                return NotFound("Bill not found");
            }

            var billingRow = billingTable.Rows[0];
            int appointmentID = Convert.ToInt32(billingRow["AppointmentID"]);
            int patientID = Convert.ToInt32(billingRow["PatientID"]);

            // Get Appointment Details
            DataTable appointmentTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Appointments_SelectByPK";
                command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentID;
                connection.Open();
                using var reader = command.ExecuteReader();
                appointmentTable = new DataTable();
                appointmentTable.Load(reader);
            }

            // Get Patient Details
            DataTable patientTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Patients_SelectByPK";
                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientID;
                connection.Open();
                using var reader = command.ExecuteReader();
                patientTable = new DataTable();
                patientTable.Load(reader);
            }

            // Get Doctor Details (from appointment)
            if (appointmentTable.Rows.Count > 0)
            {
                int doctorID = Convert.ToInt32(appointmentTable.Rows[0]["DoctorID"]);

                DataTable doctorTable;
                using (var connection = new SqlConnection(connectionString))
                {
                    using var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Doctors_SelectByPK";
                    command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorID;
                    connection.Open();
                    using var reader = command.ExecuteReader();
                    doctorTable = new DataTable();
                    doctorTable.Load(reader);
                }
                ViewData["DoctorTable"] = doctorTable;

                // Get Doctor's Department
                DataTable doctorDepartmentTable;
                using (var connection = new SqlConnection(connectionString))
                {
                    using var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = @"
                SELECT TOP 1
                    dep.DepartmentName,
                    dep.Description
                FROM DoctorDepartments dd
                INNER JOIN Departments dep ON dd.DepartmentID = dep.DepartmentID
                WHERE dd.DoctorID = @DoctorID";
                    command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorID;
                    connection.Open();
                    using var reader = command.ExecuteReader();
                    doctorDepartmentTable = new DataTable();
                    doctorDepartmentTable.Load(reader);
                }
                ViewData["DoctorDepartmentTable"] = doctorDepartmentTable;
            }

            // Get Payment History for this Bill
            DataTable paymentHistoryTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT 
                'Payment' as ActionType,
                PaidAmount as Amount,
                PaymentStatus,
                CreatedDate as ActionDate,
                'Payment processed' as Description
            FROM Billing 
            WHERE BillID = @BillID AND PaidAmount > 0
            
            UNION ALL
            
            SELECT 
                'Bill Generated' as ActionType,
                BillAmount as Amount,
                'Generated' as PaymentStatus,
                CreatedDate as ActionDate,
                'Bill created for appointment' as Description
            FROM Billing 
            WHERE BillID = @BillID
            
            ORDER BY ActionDate DESC";
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                connection.Open();
                using var reader = command.ExecuteReader();
                paymentHistoryTable = new DataTable();
                paymentHistoryTable.Load(reader);
            }

            // Get Patient's Other Bills (Outstanding)
            DataTable otherBillsTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT TOP 5
                b.BillID,
                b.AppointmentID,
                b.BillAmount,
                b.PaidAmount,
                (b.BillAmount - b.PaidAmount) as BalanceAmount,
                b.PaymentStatus,
                b.CreatedDate,
                a.AppointmentDate,
                d.Name as DoctorName
            FROM Billing b
            INNER JOIN Appointments a ON b.AppointmentID = a.AppointmentID
            INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
            WHERE a.PatientID = @PatientID 
            AND b.BillID != @BillID
            AND b.PaymentStatus IN ('Unpaid', 'Partial')
            ORDER BY b.CreatedDate DESC";
                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientID;
                command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                connection.Open();
                using var reader = command.ExecuteReader();
                otherBillsTable = new DataTable();
                otherBillsTable.Load(reader);
            }

            // Get Medical Records for context (recent)
            DataTable medicalRecordsTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT TOP 3
                mr.RecordID,
                mr.VisitDate,
                mr.Diagnosis,
                mr.Treatment,
                d.Name as DoctorName
            FROM MedicalRecords mr
            INNER JOIN Doctors d ON mr.DoctorID = d.DoctorID
            WHERE mr.PatientID = @PatientID
            ORDER BY mr.VisitDate DESC";
                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientID;
                connection.Open();
                using var reader = command.ExecuteReader();
                medicalRecordsTable = new DataTable();
                medicalRecordsTable.Load(reader);
            }

            // Calculate patient age
            if (patientTable.Rows.Count > 0)
            {
                DateTime birthDate = Convert.ToDateTime(patientTable.Rows[0]["DateOfBirth"]);
                int age = DateTime.Now.Year - birthDate.Year;
                if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                    age--;
                ViewData["PatientAge"] = age;
            }

            // Calculate bill breakdown for display
            decimal billAmount = Convert.ToDecimal(billingRow["BillAmount"]);
            decimal paidAmount = Convert.ToDecimal(billingRow["PaidAmount"]);
            decimal balanceAmount = billAmount - paidAmount;

            // Calculate tax (assuming 10% tax is included in bill amount)
            decimal subtotal = Math.Round(billAmount / 1.1m, 2);
            decimal taxAmount = billAmount - subtotal;

            ViewData["Subtotal"] = subtotal;
            ViewData["TaxAmount"] = taxAmount;
            ViewData["BalanceAmount"] = balanceAmount;

            // Pass data to view
            ViewData["BillID"] = BillID;
            ViewData["AppointmentTable"] = appointmentTable;
            ViewData["PatientTable"] = patientTable;
            ViewData["PaymentHistoryTable"] = paymentHistoryTable;
            ViewData["OtherBillsTable"] = otherBillsTable;
            ViewData["MedicalRecordsTable"] = medicalRecordsTable;

            return View(billingTable);
        }

        //[HttpPost]
        //public IActionResult ProcessPayment(int BillID, decimal PaymentAmount, string PaymentMethod, string PaymentNotes)
        //{
        //    try
        //    {
        //        string connectionString = this._configuration.GetConnectionString("ConnectionString");

        //        using (var connection = new SqlConnection(connectionString))
        //        {
        //            using var command = connection.CreateCommand();
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "SP_Billing_RecordPayment";
        //            command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
        //            command.Parameters.Add("@PaymentAmount", SqlDbType.Decimal).Value = PaymentAmount;

        //            connection.Open();
        //            using var reader = command.ExecuteReader();
        //            DataTable result = new DataTable();
        //            result.Load(reader);

        //            if (result.Rows.Count > 0)
        //            {
        //                var resultRow = result.Rows[0];
        //                return Json(new
        //                {
        //                    success = true,
        //                    message = resultRow["Message"].ToString(),
        //                    newPaidAmount = resultRow["NewPaidAmount"],
        //                    remainingBalance = resultRow["RemainingBalance"],
        //                    paymentStatus = resultRow["PaymentStatus"]
        //                });
        //            }
        //        }

        //        return Json(new { success = false, message = "Payment processing failed" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        [HttpPost]
        public IActionResult ApplyDiscount(int BillID, string DiscountType, decimal DiscountValue, string Reason)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");

                // Get current bill amount
                DataTable currentBill;
                using (var connection = new SqlConnection(connectionString))
                {
                    using var command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_Billing_GetByID";
                    command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                    connection.Open();
                    using var reader = command.ExecuteReader();
                    currentBill = new DataTable();
                    currentBill.Load(reader);
                }

                if (currentBill.Rows.Count == 0)
                {
                    return Json(new { success = false, message = "Bill not found" });
                }

                decimal currentAmount = Convert.ToDecimal(currentBill.Rows[0]["BillAmount"]);
                decimal discountAmount = 0;

                if (DiscountType == "percentage")
                {
                    discountAmount = (currentAmount * DiscountValue) / 100;
                }
                else
                {
                    discountAmount = DiscountValue;
                }

                decimal newAmount = currentAmount - discountAmount;

                // Update bill amount (you may need to create a specific stored procedure for this)
                using (var connection = new SqlConnection(connectionString))
                {
                    using var command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Billing SET BillAmount = @NewAmount WHERE BillID = @BillID";
                    command.Parameters.Add("@NewAmount", SqlDbType.Decimal).Value = newAmount;
                    command.Parameters.Add("@BillID", SqlDbType.Int).Value = BillID;
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return Json(new
                {
                    success = true,
                    message = "Discount applied successfully",
                    discountAmount = discountAmount,
                    newBillAmount = newAmount
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


       



    }

    
}