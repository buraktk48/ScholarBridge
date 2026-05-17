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

        [HttpPost]
        public IActionResult Donate(int scholarshipId, decimal amount)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            if (amount <= 0)
            {
                TempData["ErrorMessage"] = "Lütfen geçerli bir bağış miktarı giriniz.";
                return RedirectToAction("Donate");
            }

            // Ensure the donor has a DonorDetail record to prevent FK violation
            var donorDetail = _context.DonorDetails.FirstOrDefault(d => d.UserId == userId);
            if (donorDetail == null)
            {
                donorDetail = new DonorDetail { UserId = userId };
                _context.DonorDetails.Add(donorDetail);
                _context.SaveChanges();
            }

            var donation = new Donation
            {
                UserFkId = userId,
                ScholarshipFkId = scholarshipId,
                Amount = amount,
                DonatedAt = DateTime.Now,
                TransactionId = "TXN" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(),
                IsAnonymous = "Hayır"
            };

            _context.Donations.Add(donation);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Tebrikler! {amount:C2} tutarındaki bağışınız başarıyla gerçekleştirildi.";
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
