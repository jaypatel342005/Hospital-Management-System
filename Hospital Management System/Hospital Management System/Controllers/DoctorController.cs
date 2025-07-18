using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Details()
        {
            return View();
        }
        
        public IActionResult AddEdit()
        {
            return View();
        }
    }
}
