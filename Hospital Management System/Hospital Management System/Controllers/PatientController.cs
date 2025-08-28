using ClosedXML.Excel;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{

    public class PatientController : Controller
    {

        private IConfiguration _configuration;

        public PatientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Patients_SelectAll";

            connection.Open();
            using var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return View(table);
            connection.Close();
        }

        public IActionResult PatientDelete(int PatientID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Patients_DeleteByPK";
                    command.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
                    command.ExecuteNonQuery();
                }
                TempData["SuccessMessage"] = "Patient deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Patient. Please try again or contact support.";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("Index");
        }

        public IActionResult PatientSave(PatientModel patientModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            if (patientModel.PatientID == null)
                            {
                                command.CommandText = "PR_Patients_Insert";
                                TempData["SuccessMessage"] = "Patient added successfully.";
                            }
                            else
                            {
                                command.CommandText = "PR_Patients_UpdateByPK";
                                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientModel.PatientID;
                                TempData["SuccessMessage"] = "Patient updated successfully.";
                            }
                            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = patientModel.Name;
                            command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = patientModel.DateOfBirth;
                            command.Parameters.Add("@Gender", SqlDbType.VarChar).Value = patientModel.Gender;
                            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = patientModel.Email;
                            command.Parameters.Add("@Phone", SqlDbType.VarChar).Value = patientModel.Phone;
                            command.Parameters.Add("@Address", SqlDbType.VarChar).Value = patientModel.Address;
                            command.Parameters.Add("@City", SqlDbType.VarChar).Value = patientModel.City;
                            command.Parameters.Add("@State", SqlDbType.VarChar).Value = patientModel.State;
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = patientModel.IsActive;
                            command.Parameters.Add("@UserID", SqlDbType.Int).Value = patientModel.UserID;
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the Patient. Please try again.";
                Console.WriteLine(ex.ToString());
            }
            ViewBag.UserList = GetUserList();
            return View("AddEdit", patientModel);
        }


        public IActionResult PatientEdit(int PatientID)
        {
            ViewBag.UserList = GetUserList();
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Patients_SelectByPK";
            command.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();
            PatientModel patientModel = new PatientModel();
            patientModel.PatientID = Convert.ToInt32(dataTable.Rows[0]["PatientID"]);
            patientModel.Name = dataTable.Rows[0]["Name"].ToString();
            patientModel.DateOfBirth = Convert.ToDateTime(dataTable.Rows[0]["DateOfBirth"]);
            patientModel.Gender = dataTable.Rows[0]["Gender"].ToString();
            patientModel.Email = dataTable.Rows[0]["Email"].ToString();
            patientModel.Phone = dataTable.Rows[0]["Phone"].ToString();
            patientModel.Address = dataTable.Rows[0]["Address"].ToString();
            patientModel.City = dataTable.Rows[0]["City"].ToString();
            patientModel.State = dataTable.Rows[0]["State"].ToString();
            patientModel.IsActive = Convert.ToBoolean(dataTable.Rows[0]["IsActive"]);
            patientModel.UserID = Convert.ToInt32(dataTable.Rows[0]["UserID"]);
            return View("AddEdit", patientModel);
        }

        private List<SelectListItem> GetUserList()
        {
            List<SelectListItem> userList = new List<SelectListItem>();
            string connectionString = _configuration.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using SqlCommand cmd = new SqlCommand("SELECT UserID, UserName FROM [Users] WHERE IsActive = 1", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userList.Add(new SelectListItem
                    {
                        Value = reader["UserID"].ToString(),
                        Text = reader["UserName"].ToString()
                    });
                }
            }

            return userList;
        }



        public IActionResult Details(int PatientID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            // Get Patient Details using stored procedure
            DataTable patientTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Patients_SelectByPK";
                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
                connection.Open();
                using var reader = command.ExecuteReader();
                patientTable = new DataTable();
                patientTable.Load(reader);
            }

            // Check if patient exists
            if (patientTable.Rows.Count == 0)
            {
                return NotFound("Patient not found");
            }

            // Get Patient Appointments
            DataTable appointmentsTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT 
                a.AppointmentID,
                a.AppointmentDate,
                a.AppointmentStatus,
                a.Description,
                a.SpecialRemarks,
                a.TotalConsultedAmount,
                a.Created,
                d.Name as DoctorName,
                d.Specialization as DoctorSpecialization
            FROM Appointments a
            INNER JOIN Doctors d ON a.DoctorID = d.DoctorID
            WHERE a.PatientID = @PatientID
            ORDER BY a.AppointmentDate DESC";

                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
                connection.Open();
                using var reader = command.ExecuteReader();
                appointmentsTable = new DataTable();
                appointmentsTable.Load(reader);
            }

            // Get Patient Billing Information
            DataTable billingTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Billing_GetByPatientID";
                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
                connection.Open();
                using var reader = command.ExecuteReader();
                billingTable = new DataTable();
                billingTable.Load(reader);
            }

            // Get Medical Records
            DataTable medicalRecordsTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT 
                mr.RecordID,
                mr.VisitDate,
                mr.Diagnosis,
                mr.Treatment,
                mr.Created,
                d.Name as DoctorName,
                d.Specialization as DoctorSpecialization
            FROM MedicalRecords mr
            INNER JOIN Doctors d ON mr.DoctorID = d.DoctorID
            WHERE mr.PatientID = @PatientID
            ORDER BY mr.VisitDate DESC";

                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
                connection.Open();
                using var reader = command.ExecuteReader();
                medicalRecordsTable = new DataTable();
                medicalRecordsTable.Load(reader);
            }

            // Calculate billing summary
            decimal totalBilled = 0;
            decimal totalPaid = 0;
            decimal totalPending = 0;
            decimal totalOverdue = 0;

            foreach (DataRow row in billingTable.Rows)
            {
                decimal billAmount = Convert.ToDecimal(row["BillAmount"]);
                decimal paidAmount = Convert.ToDecimal(row["PaidAmount"]);
                string paymentStatus = row["PaymentStatus"].ToString();

                totalBilled += billAmount;
                totalPaid += paidAmount;

                if (paymentStatus == "Unpaid" || paymentStatus == "Partial")
                {
                    totalPending += (billAmount - paidAmount);
                }

                // You can add logic for overdue bills based on date
                // For now, keeping it as 0 as per your original data
            }

            // Pass data to view
            ViewData["PatientID"] = PatientID;
            ViewData["AppointmentsTable"] = appointmentsTable;
            ViewData["BillingTable"] = billingTable;
            ViewData["MedicalRecordsTable"] = medicalRecordsTable;
            ViewData["TotalBilled"] = totalBilled;
            ViewData["TotalPaid"] = totalPaid;
            ViewData["TotalPending"] = totalPending;
            ViewData["TotalOverdue"] = totalOverdue;

            return View(patientTable);
        }
        public IActionResult AddEdit()
        {
            ViewBag.UserList = GetUserList();
            return View("AddEdit", new PatientModel());
        }



        [Route("ExportToExcel")]
        public IActionResult ExportToExcel()
        {
            DataTable dt = RetrieveData("PR_Patients_SelectAll");

            using (var workbook = new XLWorkbook())
            {
                // Add the DataTable to a worksheet
                workbook.Worksheets.Add(dt, "States");

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string excelName = $"PatientData-{DateTime.Now:yyyy/MM/dd/HH:mm:ss}.xlsx";
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                       excelName
                    );
                }
            }
        }



        public DataTable RetrieveData(String SP, int? PKID = 0, String PKName = "")
        {
            SqlConnection conn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString"));
            conn.Open();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SP;
            if (PKID != 0)
            {
                cmd.Parameters.AddWithValue("@" + PKName, PKID);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            conn.Close();

            return dt;
        }


    }
}
