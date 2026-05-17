using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ScholarBridge.Models;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Applications()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            // Brings all applications for the organization's scholarships
            var value = context.Applications
                .Include(x => x.ScholarshipFk) // include scholarship name in the applications view
                .Include(x => x.UserFk) // include student name in the applications view 
                .Where(x => x.ScholarshipFk.UserFkId == userId)
                .OrderByDescending(x => x.AppliedAt) // bring latest applications first
                .ToList();

            return View(value);
        }

        [HttpPost]
        public IActionResult ApproveApplication(int id)
        {
            var application = context.Applications.FirstOrDefault(a => a.ApplicationId == id);
            if(application != null)
            {
                application.Status = "Onaylandı";
                context.SaveChanges();
            }
            return RedirectToAction("Applications");
        }

        [HttpPost]
        public IActionResult RejectApplication(int id)
        {
            var application = context.Applications.FirstOrDefault(a => a.ApplicationId == id);
            if(application != null)
            {
                application.Status = "Reddedildi";
                context.SaveChanges();
            }
            return RedirectToAction("Applications");
        }

        

        [HttpGet]  
        public IActionResult Profile()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            var value = context.OrganizationDetails.FirstOrDefault(x => x.UserId == userId);

            return View(value);
        }

        [HttpPost] 
        public IActionResult Profile(OrganizationDetail organization)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            // existing organization 
            var existingOrg = context.OrganizationDetails.FirstOrDefault(x => x.UserId == userId);

            if (existingOrg != null)
            {
                // only updating the form data
                existingOrg.OrgName = organization.OrgName;
                existingOrg.TaxNumber = organization.TaxNumber;
                existingOrg.Description = organization.Description;

                // we don't need to write update, entity framework tracks it 
                context.SaveChanges(); 
                
                TempData["SuccessMessage"] = "Profil bilgileri başarıyla güncellendi!";
            }
            
            return RedirectToAction("Index", "Dashboard");
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
