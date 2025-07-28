using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class DoctorDepartmentController : Controller
    {
        private IConfiguration _configuration;

        public DoctorDepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_DoctorDepartments_SelectAll";

            connection.Open();
            using var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        private List<SelectListItem> GetUserList()
        {
            List<SelectListItem> userList = new List<SelectListItem>();
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT UserID, UserName FROM [Users] WHERE IsActive = 1", conn))
                {
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
            }
            return userList;
        }

        private List<SelectListItem> GetDoctorList()
        {
            List<SelectListItem> doctorList = new List<SelectListItem>();
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT DoctorID, Name FROM [Doctors] WHERE IsActive = 1", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        doctorList.Add(new SelectListItem
                        {
                            Value = reader["DoctorID"].ToString(),
                            Text = reader["Name"].ToString()
                        });
                    }
                }
            }
            return doctorList;
        }

        private List<SelectListItem> GetDepartmentList()
        {
            List<SelectListItem> departmentList = new List<SelectListItem>();
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT DepartmentID, DepartmentName FROM [Departments] WHERE IsActive = 1", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        departmentList.Add(new SelectListItem
                        {
                            Value = reader["DepartmentID"].ToString(),
                            Text = reader["DepartmentName"].ToString()
                        });
                    }
                }
            }
            return departmentList;
        }

        public IActionResult DoctorDepartmentDelete(int DoctorDepartmentID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_DoctorDepartments_DeleteByPK";
                    command.Parameters.Add("@DoctorDepartmentID", SqlDbType.Int).Value = DoctorDepartmentID;
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Doctor-Department association deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the doctor-department association. Please try again or contact support.";
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("Index");
        }

        public IActionResult DoctorDepartmentSave(DoctorDepartmentsModel doctorDepartmentsModel)
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
                            if (doctorDepartmentsModel.DoctorDepartmentID == null)
                            {
                                command.CommandText = "PR_DoctorDepartments_Insert";
                                TempData["SuccessMessage"] = "Doctor-Department association added successfully.";
                            }
                            else
                            {
                                command.CommandText = "PR_DoctorDepartments_UpdateByPK";
                                command.Parameters.Add("@DoctorDepartmentID", SqlDbType.Int).Value = doctorDepartmentsModel.DoctorDepartmentID;
                                TempData["SuccessMessage"] = "Doctor-Department association updated successfully.";
                            }
                            command.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorDepartmentsModel.DoctorID;
                            command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = doctorDepartmentsModel.DepartmentID;
                            command.Parameters.Add("@UserID", SqlDbType.Int).Value = doctorDepartmentsModel.UserID;
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the doctor-department association. Please try again.";
                Console.WriteLine(ex.ToString());
            }

            ViewBag.UserList = GetUserList();
            ViewBag.DoctorList = GetDoctorList();
            ViewBag.DepartmentList = GetDepartmentList();
            return View("AddEdit", doctorDepartmentsModel);
        }

        public IActionResult DoctorDepartmentEdit(int DoctorDepartmentID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_DoctorDepartments_SelectByPK";
            command.Parameters.Add("@DoctorDepartmentID", SqlDbType.Int).Value = DoctorDepartmentID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            DoctorDepartmentsModel doctorDepartmentsModel = new DoctorDepartmentsModel();
            doctorDepartmentsModel.DoctorDepartmentID = Convert.ToInt32(dataTable.Rows[0]["DoctorDepartmentID"]);
            doctorDepartmentsModel.DoctorID = Convert.ToInt32(dataTable.Rows[0]["DoctorID"]);
            doctorDepartmentsModel.DepartmentID = Convert.ToInt32(dataTable.Rows[0]["DepartmentID"]);
            doctorDepartmentsModel.UserID = Convert.ToInt32(dataTable.Rows[0]["UserID"]);
            doctorDepartmentsModel.Created = Convert.ToDateTime(dataTable.Rows[0]["Created"]);
            doctorDepartmentsModel.Modified = Convert.ToDateTime(dataTable.Rows[0]["Modified"]);

            ViewBag.UserList = GetUserList();
            ViewBag.DoctorList = GetDoctorList();
            ViewBag.DepartmentList = GetDepartmentList();
            return View("AddEdit", doctorDepartmentsModel);
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult AddEdit()
        {
            ViewBag.UserList = GetUserList();
            ViewBag.DoctorList = GetDoctorList();
            ViewBag.DepartmentList = GetDepartmentList();
            return View("AddEdit", new DoctorDepartmentsModel());
        }
    }
}