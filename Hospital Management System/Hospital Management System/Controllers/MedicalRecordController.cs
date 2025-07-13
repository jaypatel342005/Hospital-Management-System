using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class MedicalRecordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
