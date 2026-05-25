using Microsoft.AspNetCore.Mvc;
using ScholarBridge.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ScholarBridge.Controllers
{
    public class AccountController : Controller
    {
        //for RAM optimization
        private readonly ScholarBridgeContext context;

        public AccountController(ScholarBridgeContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
             
        [HttpPost]
        // async Task because we use await HttpContext.SignInAsync
        public async Task<IActionResult> Login(User user)
        {
            var existingUser = context.Users.FirstOrDefault(u => u.Email == user.Email && u.PasswordHash == user.PasswordHash);

            if(existingUser!=null)
            {
                // 1. Finding the user's role from the database
                
                var userRole = context.UserRoles.FirstOrDefault(ur => ur.UserFkId == existingUser.UserId);
                string roleName = "Bilinmeyen"; // if the user's role is not found, the default role will be assigned   
                
                if (userRole != null)
                {
                    var role = context.Roles.FirstOrDefault(r => r.RoleId == userRole.RoleFkId);
                    if (role != null) roleName = role.RoleName;
                }

                // 2. we prepare the identity card information (Claims)
                // any claim is any information that we want to use later (such as name , role , id, etc)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, existingUser.UserId.ToString()), // ID
                    new Claim(ClaimTypes.Email, existingUser.Email),                      // E-mail
                    new Claim(ClaimTypes.Role, roleName)                                  // Role (Student, Org, etc.)
                };

                // 3. We prepare the identity
                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                // 4. We send the cookie to the browser (official login process)
                await HttpContext.SignInAsync("CookieAuth", principal);
                
                // 5. Redirect to Dashboard
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                // ViewBag because we are already on the login page 
                ViewBag.Error = "E-posta veya şifre hatalı!";
                return View();
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult RegisterStudent(User user,StudentDetail student)
        {
            user.CreatedAt = DateTime.Now;
            user.IsActive= true;
            context.Users.Add(user);
            context.SaveChanges();

            student.UserId = user.UserId;
            context.StudentDetails.Add(student);
            UserRole ur = new UserRole
            {
                UserFkId = user.UserId,
                RoleFkId = 2,
                
            };
            
            context.UserRoles.Add(ur);
            context.SaveChanges();
            //TempData because we are redirecting to the login page 
            TempData["SuccessMessage"] = "Öğrenci kaydınız başarıyla tamamlandı!";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult RegisterOrganization(User user,OrganizationDetail organization)
        {
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;
            context.Users.Add(user);
            context.SaveChanges();

            organization.UserId = user.UserId;
            context.OrganizationDetails.Add(organization);
            UserRole ur = new UserRole
            {
                UserFkId = user.UserId,
                RoleFkId = 3,
                
            };
            context.UserRoles.Add(ur);
            context.SaveChanges();
            TempData["SuccessMessage"] = "Vakıf kaydınız başarıyla tamamlandı!";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult RegisterDonor(User user,DonorDetail donor)
        {
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;
            context.Users.Add(user);
            context.SaveChanges();

            donor.UserId = user.UserId;
            context.DonorDetails.Add(donor);
            UserRole ur = new UserRole
            {
                UserFkId = user.UserId,
                RoleFkId = 4,
                
            };
            context.UserRoles.Add(ur);
            context.SaveChanges();
            TempData["SuccessMessage"] = "Bağışçı kaydınız başarıyla tamamlandı!";
            return RedirectToAction("Login");
        }
        
        
        






    }
}
