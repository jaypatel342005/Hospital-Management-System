using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class DoctorController : Controller
    {
        private IConfiguration _configuration;

        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            

                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using var connection = new SqlConnection(connectionString);
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Doctors_SelectAll";

                connection.Open();
                using var reader = command.ExecuteReader();
                var table = new DataTable();
                table.Load(reader);
                return View(table);
            
            
        }


        public IActionResult DoctorDelete(int DoctorID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Doctors_DeleteByPK";
                    command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DoctorID;
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Doctor deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Doctor. Please try again or contact support.";
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("Index");
        }




        public IActionResult DoctorSave(DoctorModel doctorModel)
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
                            if (doctorModel.DoctorID == null)
                            {

                                command.CommandText = "PR_Doctors_Insert";
                                TempData["SuccessMessage"] = "Doctor added successfully.";
                            }
                            else
                            {
                                command.CommandText = "PR_Doctors_UpdateByPK";
                                command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorModel.DoctorID;
                                TempData["SuccessMessage"] = "Doctor updated successfully.";
                            }
                            command.Parameters.Add("@Name", SqlDbType.VarChar).Value = doctorModel.Name;
                            command.Parameters.Add("@Phone", SqlDbType.VarChar).Value = doctorModel.Phone;
                            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = doctorModel.Email;
                            command.Parameters.Add("@Qualification", SqlDbType.VarChar).Value = doctorModel.Qualification;
                            command.Parameters.Add("@Specialization", SqlDbType.VarChar).Value = doctorModel.Specialization;
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = doctorModel.IsActive;
                            command.Parameters.Add("@UserID", SqlDbType.Int).Value = doctorModel.UserID;
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the Doctor. Please try again.";

                Console.WriteLine(ex.ToString());
            }
            ViewBag.UserList = GetUserList();
            return View("AddEdit", doctorModel);
        }




        public IActionResult DoctorEdit(int DoctorID)
        {
            ViewBag.UserList = GetUserList();

            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Doctors_SelectByPK";
            command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DoctorID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            DoctorModel doctorModel = new DoctorModel();
            doctorModel.DoctorID = Convert.ToInt32(dataTable.Rows[0]["DoctorID"]);
            doctorModel.Name = dataTable.Rows[0]["Name"].ToString();
            doctorModel.Phone = dataTable.Rows[0]["Phone"].ToString();
            doctorModel.Email = dataTable.Rows[0]["Email"].ToString();
            doctorModel.Qualification = dataTable.Rows[0]["Qualification"].ToString();
            doctorModel.Specialization = dataTable.Rows[0]["Specialization"].ToString();
            doctorModel.IsActive = Convert.ToBoolean(dataTable.Rows[0]["IsActive"]);
            doctorModel.UserID = Convert.ToInt32(dataTable.Rows[0]["UserID"]);

            return View("AddEdit", doctorModel);
        }







        public IActionResult DoctorStatusUpdate(int DoctorID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Doctors_UpdateStatusByPK";
                    command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DoctorID;
                    command.ExecuteNonQuery();
                }

               
            }
            catch (Exception ex)
            {
               
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("Index");
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

        public IActionResult Details(int DoctorID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            // Get Doctor Details using the stored procedure
            DataTable doctorTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Doctors_SelectByPK";
                command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DoctorID;
                connection.Open();
                using var reader = command.ExecuteReader();
                doctorTable = new DataTable();
                doctorTable.Load(reader);
            }

            // Get Doctor's Assigned Departments
            DataTable departmentsTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT 
                d.DepartmentID,
                d.DepartmentName,
                d.Description,
                dd.Created as AssignedDate,
                u.UserName as AssignedBy
            FROM Departments d
            INNER JOIN DoctorDepartments dd ON d.DepartmentID = dd.DepartmentID
            INNER JOIN Users u ON dd.UserID = u.UserID
            WHERE dd.DoctorID = @DoctorID
            AND d.IsActive = 1
            ORDER BY dd.Created DESC";
                command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DoctorID;
                connection.Open();
                using var reader = command.ExecuteReader();
                departmentsTable = new DataTable();
                departmentsTable.Load(reader);
            }

            // Get Recent Appointments for this Doctor
            DataTable appointmentsTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT TOP 20
                a.AppointmentID,
                a.AppointmentDate,
                a.AppointmentStatus,
                a.TotalConsultedAmount,
                p.Name as PatientName,
                p.Phone as PatientPhone,
                a.Description,
                a.SpecialRemarks
            FROM Appointments a
            INNER JOIN Patients p ON a.PatientID = p.PatientID
            WHERE a.DoctorID = @DoctorID
            ORDER BY a.AppointmentDate DESC";
                command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DoctorID;
                connection.Open();
                using var reader = command.ExecuteReader();
                appointmentsTable = new DataTable();
                appointmentsTable.Load(reader);
            }

            // Get Doctor Statistics
            DataTable statsTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT 
                COUNT(*) as TotalAppointments,
                COUNT(DISTINCT a.PatientID) as UniquePatients,
                SUM(CASE WHEN CAST(a.AppointmentDate AS DATE) = CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END) as TodaysAppointments,
                (SELECT COUNT(DISTINCT dd.DepartmentID) FROM DoctorDepartments dd WHERE dd.DoctorID = @DoctorID) as TotalDepartments
            FROM Appointments a
            WHERE a.DoctorID = @DoctorID";
                command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DoctorID;
                connection.Open();
                using var reader = command.ExecuteReader();
                statsTable = new DataTable();
                statsTable.Load(reader);
            }

            // Pass data to the view
            ViewData["DoctorID"] = DoctorID;
            ViewData["DepartmentsTable"] = departmentsTable;
            ViewData["AppointmentsTable"] = appointmentsTable;
            ViewData["StatsTable"] = statsTable;

            return View(doctorTable);
        }

        public IActionResult AddEdit()
        {
            ViewBag.UserList = GetUserList();

            return View("AddEdit", new DoctorModel());
        }
    }
}
