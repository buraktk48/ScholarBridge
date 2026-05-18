using Microsoft.AspNetCore.Mvc;
using ScholarBridge.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public IActionResult Profile()
        {
            //getting user id instead of profile(int id)
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            //fetch studentdetails from database
            var studentDetail = context.StudentDetails.FirstOrDefault(s => s.UserId == userId);
            
            // if studentDetail is null, create a new one
            if (studentDetail == null)
            {
                studentDetail = new StudentDetail();
            }

            return View(studentDetail);
        }

        [HttpPost]
        public IActionResult Profile(StudentDetail updatedProfile, IFormFile? TranscriptFile, IFormFile? StudentCertificateFile, IFormFile? DormitoryFile)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            // finding the existing record in the database
            var existingStudent = context.StudentDetails.FirstOrDefault(s => s.UserId == userId);
            //Security control
            
            if (existingStudent == null)
            {
                existingStudent = new StudentDetail { UserId = userId };
                context.StudentDetails.Add(existingStudent);
            }

            // updating the existing record with the new data
            existingStudent.StudentName = updatedProfile.StudentName;
            existingStudent.StudentSurname = updatedProfile.StudentSurname;
            existingStudent.Gpa = updatedProfile.Gpa;
            existingStudent.Department = updatedProfile.Department;
            existingStudent.UniversityName = updatedProfile.UniversityName;
            existingStudent.FamilyIncome = updatedProfile.FamilyIncome;
            existingStudent.Grade = updatedProfile.Grade;

            // documents folder for saving uploaded files
            string documentFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documents");
            if (!Directory.Exists(documentFolder)) Directory.CreateDirectory(documentFolder);

            // Transcript
            //Security controls
            if (TranscriptFile != null && TranscriptFile.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString("N").Substring(0, 12) + Path.GetExtension(TranscriptFile.FileName);
                using (var fileStream = new FileStream(Path.Combine(documentFolder, uniqueFileName), FileMode.Create))
                {
                    TranscriptFile.CopyTo(fileStream);
                }
                existingStudent.TranscriptPath = "/documents/" + uniqueFileName;
            }

            // 2. Student Certificate
            if (StudentCertificateFile != null && StudentCertificateFile.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString("N").Substring(0, 12) + Path.GetExtension(StudentCertificateFile.FileName);
                using (var fileStream = new FileStream(Path.Combine(documentFolder, uniqueFileName), FileMode.Create))
                {
                    StudentCertificateFile.CopyTo(fileStream);
                }
                existingStudent.StudentCertificatePath = "/documents/" + uniqueFileName;
            }

            // 3. Dormitory/Accommodation Document
            if (DormitoryFile != null && DormitoryFile.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString("N").Substring(0, 12) + Path.GetExtension(DormitoryFile.FileName);
                using (var fileStream = new FileStream(Path.Combine(documentFolder, uniqueFileName), FileMode.Create))
                {
                    DormitoryFile.CopyTo(fileStream);
                }
                existingStudent.DormitoryPath = "/documents/" + uniqueFileName;
            }

            // we cant use update**
            //Entity Framework will automatically update 
            

            context.SaveChanges();

            TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi!";
            return RedirectToAction("Index", "Dashboard");
        }

        

        [HttpGet]
        public IActionResult MyApplications()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out int userId);

            // with include we get the scholarship info
            var myApplications = context.Applications
                .Include(a => a.ScholarshipFk) 
                .Where(a => a.UserFkId == userId)
                .OrderByDescending(a => a.AppliedAt)
                .ToList();

            return View(myApplications);
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
                        string uniqueFileName = Guid.NewGuid().ToString("N").Substring(0, 12) + Path.GetExtension(item.FileName);
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
