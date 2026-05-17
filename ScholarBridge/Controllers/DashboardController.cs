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

            
            if (User.IsInRole("Donor"))
            {
                // show active scholarships (IsExpired != true)
                var donorScholarships = context.Scholarships
                                            .Where(s => s.IsExpired != true)
                                            .ToList();
                ViewBag.DonorScholarships = donorScholarships;

                // show the count of active scholarships as DonationCount
                ViewBag.DonationCount = context.Scholarships.Count(s => s.IsExpired != true);

                // calculate sum of donations safely
                ViewBag.TotalDonated = context.Donations
                                            .Where(d => d.UserFkId == userId)
                                            .Sum(d => d.Amount) ?? 0;
            }
            else if (User.IsInRole("Organization"))
            {
                ViewBag.ScholarshipCount = context.Scholarships.Count(x => x.UserFkId == userId);
                ViewBag.ApplicationCount = context.Applications.Count(x => x.ScholarshipFk.UserFkId == userId && x.Status == "Bekliyor");
            }
            
            else if (User.IsInRole("Student"))
            {
                // Active scholarships count
                ViewBag.ActiveScholarships = context.Scholarships.Count(x => x.IsExpired == false && (x.Deadline == null || x.Deadline > DateOnly.FromDateTime(DateTime.Now)));

                // Pending applications count
                ViewBag.PendingApplications = context.Applications.Count(x => x.UserFkId == userId && x.Status == "Bekliyor");

                // Approved applications count
                ViewBag.ApprovedApplications = context.Applications.Count(x => x.UserFkId == userId && x.Status == "Onaylandı");
            }

            return View();
        }
    }
}
