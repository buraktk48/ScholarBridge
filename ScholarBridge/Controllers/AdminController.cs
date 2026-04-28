using Microsoft.AspNetCore.Mvc;

namespace ScholarBridge.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
