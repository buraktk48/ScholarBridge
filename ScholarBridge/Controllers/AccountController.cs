using Microsoft.AspNetCore.Mvc;

namespace ScholarBridge.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
