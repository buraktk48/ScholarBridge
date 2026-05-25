using Microsoft.AspNetCore.Mvc;
using ScholarBridge.Models;

namespace ScholarBridge.Controllers
{
    public class DonorController : Controller
    {
        private readonly ScholarBridgeContext _context;

        public DonorController(ScholarBridgeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Donate()
        {
            var values = _context.Scholarships.Where(x => x.IsExpired != true).ToList();
            return View(values);
        }

        

        public IActionResult Index()
        {
            return View();
        }
    }
}
