using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ScholarBridge.Models;
using System.Security.Claims;

namespace ScholarBridge.Controllers
{
    [Authorize] // only logged in users can access this page
    public class DashboardController : Controller
    {
        private readonly ScholarBridgeContext context;

        
        public DashboardController(ScholarBridgeContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            
            if (User.IsInRole("Organization"))
            {
                
                ViewBag.ScholarshipCount = context.Scholarships.Count(x => x.UserFkId == userId);
                
               
                ViewBag.ApplicationCount = 0; 
            }
            
            else if (User.IsInRole("Student"))
            {
                
                ViewBag.AppliedScholarshipCount = context.Applications.Count(x => x.UserFkId == userId);
            }

            return View();
        }
    }
}
