using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ScholarBridge.Models;
using System.Linq;
using System;

namespace ScholarBridge.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ScholarBridgeContext context;

        public AdminController(ScholarBridgeContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            // Total Counts
            ViewBag.TotalStudents = context.StudentDetails.Count();
            ViewBag.TotalOrganizations = context.OrganizationDetails.Count();
            ViewBag.TotalDonors = context.DonorDetails.Count();
            ViewBag.TotalUsers = context.Users.Count();

            // Financial Stats
            ViewBag.TotalDonationCount = context.Donations.Count();
            ViewBag.TotalDonatedAmount = context.Donations.Sum(d => d.Amount) ?? 0;

            // Scholarships
            ViewBag.TotalScholarships = context.Scholarships.Count();
            ViewBag.ActiveScholarships = context.Scholarships.Count(s => s.IsExpired != true);

            // Recent 5 Donations with Donor Details and Scholarship Title
            var recentDonations = context.Donations
                .Include(d => d.ScholarshipFk)
                .Include(d => d.UserFk)
                .OrderByDescending(d => d.DonatedAt)
                .Take(5)
                .ToList();

            // Recent 5 Applications with Student and Scholarship Info
            var recentApplications = context.Applications
                .Include(a => a.ScholarshipFk)
                .Include(a => a.UserFk)
                .OrderByDescending(a => a.AppliedAt)
                .Take(5)
                .ToList();

            ViewBag.RecentDonations = recentDonations;
            ViewBag.RecentApplications = recentApplications;

            return View();
        }

        [HttpGet]
        public IActionResult Donations()
        {
            // Fetch all donations with their corresponding relationships for a detailed report
            var allDonations = context.Donations
                .Include(d => d.ScholarshipFk)
                .Include(d => d.UserFk)
                .OrderByDescending(d => d.DonatedAt)
                .ToList();

            return View(allDonations);
        }


        [HttpGet]
        public IActionResult StuList()
        {
            var values = context.UserRoles
                .Include(p => p.UserFk)
                .ThenInclude(u => u.StudentDetail)
                .Where(p => p.RoleFkId == 2)
                .ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult OrgList()
        {
            var values = context.UserRoles
                .Include(p => p.UserFk)
                .ThenInclude(z => z.OrganizationDetail)
                .Where(r => r.RoleFkId == 3)
                .ToList();
            return View(values);
        }

        [HttpPost]
        public IActionResult ApproveOrg(int userId)
        {
            var org = context.OrganizationDetails.FirstOrDefault(o => o.UserId == userId);
            if (org != null)
            {
                org.IsVerified = true;
                context.SaveChanges();
            }
            return RedirectToAction("OrgList");
        }

        [HttpPost]
        public IActionResult RejectOrg(int userId)
        {
            var org = context.OrganizationDetails.FirstOrDefault(o => o.UserId == userId);
            if (org != null)
            {
                org.IsVerified = false;
                context.SaveChanges();
            }
            return RedirectToAction("OrgList");
        }

        [HttpGet]

        public IActionResult DonList()
        {
            var values = context.UserRoles
                .Include(p => p.UserFk)
                .ThenInclude(z=> z.DonorDetail)
                .Where(r=> r.RoleFkId == 4)
                .ToList();
            return View(values);
        }


    }
}
