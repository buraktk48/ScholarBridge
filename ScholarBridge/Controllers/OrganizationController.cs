using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ScholarBridge.Models;

namespace ScholarBridge.Controllers
{
    [Authorize(Roles = "Organization")] // only organizations can access this controller
    public class OrganizationController : Controller
    {
        private readonly ScholarBridgeContext context;

        public OrganizationController(ScholarBridgeContext _context)
        {
            context = _context;
        }

        [HttpGet]
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Scholarship scholarship)
        {
            // getting the user id from the claim, the system knows who is "User" and stores their info in the claim 
            // cookies only store string data types
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // if tryParse is true userId = parsed value, if false userId = 0 
            scholarship.UserFkId = int.TryParse(userIdString, out int userId) ? userId : (int?)null; 

            scholarship.IsExpired = false; 
            context.Scholarships.Add(scholarship);
            context.SaveChanges();
            TempData["SuccessMessage"] = "Burs kaydı başarıyla tamamlandı!";
            return RedirectToAction("Index", "Dashboard");
        }

        
        [HttpGet]
        public IActionResult ScholarshipList()
        {
            // We need to get the ID of the user who is currently logged in, so that we can list their specific scholarships. 
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId); 

            var values = context.Scholarships.Where(x => x.UserFkId == userId).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult EditScholarship(int id)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            var values = context.Scholarships.Where(x => x.UserFkId == userId && x.ScholarshipId == id).FirstOrDefault();
            return View(values);

        }
        
        [HttpPost]
        public IActionResult EditScholarship(Scholarship scholarship)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            context.Scholarships.Update(scholarship); 
            context.SaveChanges();
            TempData["SuccessMessage"] = "Burs ilanı başarıyla güncellendi!";
            return RedirectToAction("ScholarshipList");

        
        }

        [HttpPost]
        public IActionResult DeleteScholarship(int id)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            var values = context.Scholarships.Where(x => x.UserFkId == userId && x.ScholarshipId == id).FirstOrDefault();
            //Security control
            if (values != null) 
            {
                context.Scholarships.Remove(values);
                context.SaveChanges();
                TempData["SuccessMessage"] = "Burs ilanı başarıyla silindi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Burs ilanı bulunamadı!";
            }
            return RedirectToAction("ScholarshipList"); 
        }
    


    }
}
