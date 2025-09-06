using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    [CheckAccess]
 
    public class DepartmentController : Controller
    {
        private IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public IActionResult Index()
        //{

        //        string connectionString = this._configuration.GetConnectionString("ConnectionString");
        //        using var connection = new SqlConnection(connectionString);
        //        using var command = connection.CreateCommand();
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "PR_Departments_SelectAll";

        //        connection.Open();
        //        using var reader = command.ExecuteReader();
        //        var table = new DataTable();
        //        table.Load(reader);
        //        return View(table);

        //}

        public IActionResult Index(string DepartmentName = "", bool? IsActive = null)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlConnection objConn = new SqlConnection(this._configuration.GetConnectionString("ConnectionString"));
                objConn.Open();

                SqlCommand objCmd = objConn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Departments_SelectAll";

                objCmd.Parameters.Add("@DepartmentName", SqlDbType.VarChar).Value = DepartmentName ?? "";
                objCmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = (object)IsActive ?? DBNull.Value;

                SqlDataAdapter objAdapter = new SqlDataAdapter(objCmd);
                objAdapter.Fill(dt);

                objConn.Close();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            ViewBag.DepartmentName = DepartmentName;
            ViewBag.IsActive = IsActive;

            return View(dt);
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



        [EncryptedActionParameter]
        public IActionResult DepartmentDelete(int DepartmentID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Departments_DeleteByPK";
                    command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = DepartmentID;
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Department deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the department. Please try again or contact support.";
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("Index");
        }



        [EncryptedActionParameter]
        public IActionResult DepartmentSave(DepartmentsModel departmentsModel)
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
                            if (departmentsModel.DepartmentID == null)
                            {
                                command.CommandText = "PR_Departments_Insert";
                                TempData["SuccessMessage"] = "Department added successfully.";
                            }
                            else
                            {
                                command.CommandText = "PR_Departments_UpdateByPK";
                                command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = departmentsModel.DepartmentID;
                                TempData["SuccessMessage"] = "Department updated successfully.";
                            }
                            command.Parameters.Add("@DepartmentName", SqlDbType.VarChar).Value = departmentsModel.DepartmentName;
                            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = departmentsModel.Description ?? (object)DBNull.Value;
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = departmentsModel.IsActive;
                            command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID();
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the department. Please try again.";
                
                Console.WriteLine(ex.ToString());
            }
            ViewBag.UserList = GetUserList();
            return View("AddEdit", departmentsModel);
        }


        [EncryptedActionParameter]
        public IActionResult DepartmentEdit(int DepartmentID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "PR_Departments_SelectByPK";
            command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = DepartmentID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            DepartmentsModel departmentsModel = new DepartmentsModel();
            departmentsModel.DepartmentID = Convert.ToInt32(dataTable.Rows[0]["DepartmentID"]);
            departmentsModel.DepartmentName = dataTable.Rows[0]["DepartmentName"].ToString();
            departmentsModel.Description = dataTable.Rows[0]["Description"].ToString();
            departmentsModel.IsActive = Convert.ToBoolean(dataTable.Rows[0]["IsActive"]);
            departmentsModel.UserID = Convert.ToInt32(dataTable.Rows[0]["UserID"]);

            ViewBag.UserList = GetUserList();
            return View("AddEdit", departmentsModel);
        }



        [EncryptedActionParameter]
        public IActionResult Details(int DepartmentID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            // Get Department Details
            DataTable departmentTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Departments_SelectByPK";
                command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = DepartmentID;
                connection.Open();
                using var reader = command.ExecuteReader();
                departmentTable = new DataTable();
                departmentTable.Load(reader);
            }

            // Get Department Staff (Doctors)
            DataTable staffTable;
            using (var connection = new SqlConnection(connectionString))
            {
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = @"
            SELECT 
                d.DoctorID,
                d.Name,
                d.Phone,
                d.Email,
                d.Qualification,
                d.Specialization,
                d.IsActive,
                d.Created,
                u.UserName as CreatedBy
            FROM Doctors d
            INNER JOIN DoctorDepartments dd ON d.DoctorID = dd.DoctorID
            INNER JOIN Users u ON d.UserID = u.UserID
            WHERE dd.DepartmentID = @DepartmentID
            AND d.IsActive = 1
            ORDER BY d.Name";
                command.Parameters.Add("@DepartmentID", SqlDbType.Int).Value = DepartmentID;
                connection.Open();
                using var reader = command.ExecuteReader();
                staffTable = new DataTable();
                staffTable.Load(reader);
            }

            // Pass data to the view
            ViewData["DepartmentID"] = DepartmentID;
            ViewData["StaffTable"] = staffTable;

            return View(departmentTable);
        }


        [EncryptedActionParameter]
        public IActionResult AddEdit()
        {
            ViewBag.UserList = GetUserList();
            return View("AddEdit", new DepartmentsModel());
        }

       
    }
}
 