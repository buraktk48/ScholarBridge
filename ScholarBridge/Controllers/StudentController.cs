using Microsoft.AspNetCore.Mvc;
using ScholarBridge.Models;
using Microsoft.AspNetCore.Authorization;

namespace ScholarBridge.Controllers
{
    [Authorize(Roles = "Student")] // only students can access this controller
    public class StudentController : Controller
    {
        private readonly ScholarBridgeContext context;

        public StudentController(ScholarBridgeContext _context)
        {
            context = _context;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Scholarships()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            var values = context.Scholarships.ToList();
            
            // student's previous applications, only id
            ViewBag.AppliedScholarshipIds = context.Applications.Where(a => a.UserFkId == userId).Select(a => a.ScholarshipFkId).ToList();

            return View(values);
        }

        
        [HttpGet]
        public IActionResult Application(int id)
        {
            
            var chosenScholarship = context.Scholarships.FirstOrDefault(s => s.ScholarshipId == id);
            
            return View(chosenScholarship);
        }
        [HttpPost]
        public IActionResult Application(Application application,List<IFormFile> ExtraDocuments)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);
            
            application.UserFkId = userId;
            application.AppliedAt = DateTime.Now;
            application.Status = "Bekliyor";
            
            context.Applications.Add(application);
            // Downloading files and saving them to database
            if (ExtraDocuments != null)
            {
                // Creating a folder to save the files
                //find the path to the wwwroot/documents folder in the project
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents");
                if (!Directory.Exists(uploadsFolder)) 
                { 
                    Directory.CreateDirectory(uploadsFolder); 
                }

                foreach(var item in ExtraDocuments)
                {
                    if (item.Length > 0)
                    {
                        // Generate unique things to avoid same filename issue
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file to the server
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(fileStream); 
                        }

                        // creating a studentfile object for database 
                        StudentFile sf = new StudentFile();
                        sf.UserId = userId;
                        sf.FileName = item.FileName;
                        sf.FilePath = "/documents/" + uniqueFileName; // the path of the file in the server
                        
                        context.StudentFiles.Add(sf);
                    }
                }
            }
            context.SaveChanges();

            TempData["SuccessMessage"] = "Başvurunuz ve belgeleriniz başarıyla kaydedilmiştir.";

            
            return RedirectToAction("Scholarships");
        }




    }
}
