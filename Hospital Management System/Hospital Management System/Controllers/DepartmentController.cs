using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class DepartmentController : Controller
    {
        private IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public IActionResult Index()
        {
           
                string connectionString = this._configuration.GetConnectionString("ConnectionString");
                using var connection = new SqlConnection(connectionString);
                using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Departments_SelectAll";
                
                connection.Open();
                using var reader = command.ExecuteReader();
                var table = new DataTable();
                table.Load(reader);
                return View(table);
           
        }


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
 