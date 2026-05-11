using Microsoft.AspNetCore.Mvc;

namespace ScholarBridge.Controllers
{
    public class DonorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
