using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ScholarBridge.Models;

namespace ScholarBridge.Controllers
{
    [Authorize(Roles = "Organization")] // Sadece Vakıf üyeleri erişebilir
    public class OrganizationController : Controller
    {
        private readonly ScholarBridgeContext context;

        public OrganizationController(ScholarBridgeContext _context)
        {
            context = _context;
        }
        
        public IActionResult Create()
        {
            return View();
        }
    }
}
