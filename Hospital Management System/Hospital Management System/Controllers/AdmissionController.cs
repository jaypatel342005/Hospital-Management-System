using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management_System.Controllers
{
    public class AdmissionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
