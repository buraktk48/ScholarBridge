using Microsoft.AspNetCore.Mvc;

namespace ScholarBridge.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
