using ClosedXML.Excel;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    [CheckAccess]
    public class AppointmentController : Controller
    {

        private IConfiguration _configuration;

        public AppointmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Appointments_SelectAll";

            connection.Open();
            using var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return View(table);
            connection.Close();
        }


        [EncryptedActionParameter]
        public IActionResult AppointmentDelete(int AppointmentID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Appointments_DeleteByPK";
                    command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Appointment deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Appointment. Please try again or contact support.";
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("Index");
        }


        [EncryptedActionParameter]
        public IActionResult AppointmentSave(AppointmentModel appointmentModel)
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
                            if (appointmentModel.AppointmentID == null)
                            {
                                command.CommandText = "PR_Appointments_Insert";
                                TempData["SuccessMessage"] = "Appointment added successfully.";
                            }
                            else
                            {
                                command.CommandText = "PR_Appointments_UpdateByPK";
                                command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = appointmentModel.AppointmentID;
                                TempData["SuccessMessage"] = "Appointment updated successfully.";
                            }
                            command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = appointmentModel.DoctorID;
                            command.Parameters.Add("@PatientID", SqlDbType.Int).Value = appointmentModel.PatientID;
                            command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID();
                            command.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = appointmentModel.AppointmentDate;
                            command.Parameters.Add("@AppointmentStatus", SqlDbType.VarChar).Value = appointmentModel.AppointmentStatus;
                            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = appointmentModel.Description ?? (object)DBNull.Value;
                            command.Parameters.Add("@SpecialRemarks", SqlDbType.VarChar).Value = appointmentModel.SpecialRemarks ?? (object)DBNull.Value;
                            command.Parameters.Add("@TotalConsultedAmount", SqlDbType.Decimal).Value = appointmentModel.TotalConsultedAmount ?? (object)DBNull.Value;
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the Appointment. Please try again.";
                Console.WriteLine(ex.ToString());
            }

            // Load dropdown lists for the form
            ViewBag.DoctorList = GetDoctorList();
            ViewBag.PatientList = GetPatientList();
            ViewBag.UserList = GetUserList();
            ViewBag.StatusList = GetAppointmentStatusList();

            return View("AddEdit", appointmentModel);
        }


        [EncryptedActionParameter]
        public IActionResult AppointmentEdit(int AppointmentID)
        {
            // Load dropdown lists for the form
            ViewBag.DoctorList = GetDoctorList();
            ViewBag.PatientList = GetPatientList();
            ViewBag.UserList = GetUserList();
            ViewBag.StatusList = GetAppointmentStatusList();

            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "PR_Appointments_SelectByPK";
                    command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        if (dataTable.Rows.Count == 0)
                        {
                            TempData["ErrorMessage"] = "Appointment not found.";
                            return RedirectToAction("Index");
                        }

                        AppointmentModel appointmentModel = new AppointmentModel();
                        DataRow row = dataTable.Rows[0];

                        appointmentModel.AppointmentID = Convert.ToInt32(row["AppointmentID"]);
                        appointmentModel.DoctorID = Convert.ToInt32(row["DoctorID"]);
                        appointmentModel.PatientID = Convert.ToInt32(row["PatientID"]);
                        appointmentModel.UserID = Convert.ToInt32(row["UserID"]);
                        appointmentModel.AppointmentDate = Convert.ToDateTime(row["AppointmentDate"]);
                        appointmentModel.AppointmentStatus = row["AppointmentStatus"].ToString();
                        appointmentModel.Description = row["Description"]?.ToString();
                        appointmentModel.SpecialRemarks = row["SpecialRemarks"]?.ToString();
                        appointmentModel.Created = row["Created"] != DBNull.Value ? Convert.ToDateTime(row["Created"]) : (DateTime?)null;
                        appointmentModel.Modified = row["Modified"] != DBNull.Value ? Convert.ToDateTime(row["Modified"]) : (DateTime?)null;
                        appointmentModel.TotalConsultedAmount = row["TotalConsultedAmount"] != DBNull.Value ? Convert.ToDecimal(row["TotalConsultedAmount"]) : (decimal?)null;

                        // Populate joined fields if they exist in the result set
                        if (dataTable.Columns.Contains("DoctorName"))
                            appointmentModel.DoctorName = row["DoctorName"]?.ToString();
                        if (dataTable.Columns.Contains("PatientName"))
                            appointmentModel.PatientName = row["PatientName"]?.ToString();
                        if (dataTable.Columns.Contains("UserName"))
                            appointmentModel.UserName = row["UserName"]?.ToString();

                        return View("AddEdit", appointmentModel);
                    }
                }
            }
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

        private List<SelectListItem> GetDoctorList()
        {
            List<SelectListItem> userList = new List<SelectListItem>();
            string connectionString = _configuration.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using SqlCommand cmd = new SqlCommand("SELECT DoctorID, Name FROM [Doctors] WHERE IsActive = 1", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userList.Add(new SelectListItem
                    {
                        Value = reader["DoctorID"].ToString(),
                        Text = reader["Name"].ToString()
                    });
                }
            }

            return userList;
        }


        private List<SelectListItem> GetPatientList()
        {
            List<SelectListItem> userList = new List<SelectListItem>();
            string connectionString = _configuration.GetConnectionString("ConnectionString");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using SqlCommand cmd = new SqlCommand("SELECT PatientID, Name FROM [Patients] WHERE IsActive = 1", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userList.Add(new SelectListItem
                    {
                        Value = reader["PatientID"].ToString(),
                        Text = reader["Name"].ToString()
                    });
                }
            }

            return userList;
        }


        private List<SelectListItem> GetAppointmentStatusList()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Value = "Scheduled", Text = "Scheduled" },
        new SelectListItem { Value = "Completed", Text = "Completed" },
        new SelectListItem { Value = "Cancelled", Text = "Cancelled" },
        new SelectListItem { Value = "Pending", Text = "Pending" }
    };
        }


        //private List<SelectListItem> GetUserList()
        //{
        //    List<SelectListItem> userList = new List<SelectListItem>();
        //    string connectionString = _configuration.GetConnectionString("ConnectionString");

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        using SqlCommand cmd = new SqlCommand("SELECT UserID, UserName FROM [Users] WHERE IsActive = 1", conn);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            userList.Add(new SelectListItem
        //            {
        //                Value = reader["UserID"].ToString(),
        //                Text = reader["UserName"].ToString()
        //            });
        //        }
        //    }

        //    return userList;
        //}


        [EncryptedActionParameter]
        public IActionResult Details(int AppointmentID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            // Get Appointment Details using existing stored procedure
            DataTable appointmentTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Appointments_SelectByPK";
                command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
                connection.Open();
                using var reader = command.ExecuteReader();
                appointmentTable = new DataTable();
                appointmentTable.Load(reader);
            }

            // Check if appointment exists
            if (appointmentTable.Rows.Count == 0)
            {
                return NotFound("Appointment not found");
            }

            var appointmentRow = appointmentTable.Rows[0];
            int patientID = Convert.ToInt32(appointmentRow["PatientID"]);
            int doctorID = Convert.ToInt32(appointmentRow["DoctorID"]);

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

            // Get Doctor Details
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

            // Get Patient's Previous Appointments (Medical History)
            DataTable previousAppointmentsTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT TOP 5
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
            AND a.AppointmentID != @AppointmentID
            AND a.AppointmentStatus IN ('Completed', 'Scheduled')
            ORDER BY a.AppointmentDate DESC";

                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientID;
                command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
                connection.Open();
                using var reader = command.ExecuteReader();
                previousAppointmentsTable = new DataTable();
                previousAppointmentsTable.Load(reader);
            }

            // Get Patient's Medical Records
            DataTable medicalRecordsTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT TOP 5
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

                command.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientID;
                connection.Open();
                using var reader = command.ExecuteReader();
                medicalRecordsTable = new DataTable();
                medicalRecordsTable.Load(reader);
            }

            // Get Billing Information for this appointment
            DataTable billingTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Billing_GetByAppointment";
                command.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
                connection.Open();
                using var reader = command.ExecuteReader();
                billingTable = new DataTable();
                billingTable.Load(reader);
            }

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

            // Calculate patient age
            DateTime birthDate = Convert.ToDateTime(patientTable.Rows[0]["DateOfBirth"]);
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.DayOfYear < birthDate.DayOfYear)
                age--;

            // Pass data to view
            ViewData["AppointmentID"] = AppointmentID;
            ViewData["PatientTable"] = patientTable;
            ViewData["DoctorTable"] = doctorTable;
            ViewData["PreviousAppointmentsTable"] = previousAppointmentsTable;
            ViewData["MedicalRecordsTable"] = medicalRecordsTable;
            ViewData["BillingTable"] = billingTable;
            ViewData["DoctorDepartmentTable"] = doctorDepartmentTable;
            ViewData["PatientAge"] = age;

            return View(appointmentTable);
        }

        [EncryptedActionParameter]
        public IActionResult AddEdit(int? PatientID = null)
        {
            ViewBag.StatusList = GetAppointmentStatusList();
            ViewBag.DoctorList = GetDoctorList();
            ViewBag.PatientList = GetPatientList();
            ViewBag.UserList = GetUserList();

            
            var appointmentModel = new AppointmentModel();
            if (PatientID.HasValue)
            {
                appointmentModel.PatientID = PatientID.Value;
            }

            return View("AddEdit", appointmentModel);
        }



            [Route("ExportAppointmentsToExcel")]
            public IActionResult ExportAppointmentsToExcel()
            {
                DataTable dt = RetrieveData("PR_Appointments_SelectAll");

                using (var workbook = new XLWorkbook())
                {
                    // Add the DataTable to a worksheet
                    workbook.Worksheets.Add(dt, "States");

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        string excelName = $"AppointmentsData-{DateTime.Now:yyyy/MM/dd/HH:mm:ss}.xlsx";
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
