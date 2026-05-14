using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ScholarBridge.Controllers
{
    [Authorize] // only logged in users can access this page
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
