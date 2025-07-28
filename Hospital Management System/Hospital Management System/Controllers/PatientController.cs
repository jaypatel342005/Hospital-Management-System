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



        public IActionResult Details()
        {
            return View();
        }
        public IActionResult AddEdit()
        {
            ViewBag.UserList = GetUserList();
            return View("AddEdit", new PatientModel());
        }
       
    }
}
