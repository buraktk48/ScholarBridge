using Microsoft.AspNetCore.Mvc;

namespace ScholarBridge.Controllers
{
    public class OrganizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
