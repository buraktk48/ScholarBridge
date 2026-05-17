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
        private readonly ScholarBridgeContext _context;

        public AdminController(ScholarBridgeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Total Counts
            ViewBag.TotalStudents = _context.StudentDetails.Count();
            ViewBag.TotalOrganizations = _context.OrganizationDetails.Count();
            ViewBag.TotalDonors = _context.DonorDetails.Count();
            ViewBag.TotalUsers = _context.Users.Count();

            // Financial Stats
            ViewBag.TotalDonationCount = _context.Donations.Count();
            ViewBag.TotalDonatedAmount = _context.Donations.Sum(d => d.Amount) ?? 0;

            // Scholarships
            ViewBag.TotalScholarships = _context.Scholarships.Count();
            ViewBag.ActiveScholarships = _context.Scholarships.Count(s => s.IsExpired != true);

            // Recent 5 Donations with Donor Details and Scholarship Title
            var recentDonations = _context.Donations
                .Include(d => d.ScholarshipFk)
                .Include(d => d.UserFk)
                .OrderByDescending(d => d.DonatedAt)
                .Take(5)
                .ToList();

            // Recent 5 Applications with Student and Scholarship Info
            var recentApplications = _context.Applications
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
            var allDonations = _context.Donations
                .Include(d => d.ScholarshipFk)
                .Include(d => d.UserFk)
                .OrderByDescending(d => d.DonatedAt)
                .ToList();

            return View(allDonations);
        }
    }
}
