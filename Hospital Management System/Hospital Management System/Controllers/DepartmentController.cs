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
 