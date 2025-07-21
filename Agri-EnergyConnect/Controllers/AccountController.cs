using Microsoft.AspNetCore.Mvc;
using Agri_EnergyConnect.Models;
using System.Security.Cryptography;
using System.Text;

namespace Agri_EnergyConnect.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appUser = new AppUser
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = HashPassword(model.Password),
                    Role = "User" 
                };

                await DatabaseHelper.Instance.AddUser(appUser);
                TempData["SuccessMessage"] = "Account created successfully";
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = HashPassword(model.Password);
                var user = await DatabaseHelper.Instance.GetUserByEmail(model.Email);

                if (user != null && user.Password == hashedPassword)
                {
                    HttpContext.Session.SetString("UserEmail", model.Email);
                    HttpContext.Session.SetString("UserRole", user.Role);

                    return user.Role == "Admin"
                        ? RedirectToAction("Dashboard", "Admin")
                        : RedirectToAction("UserHome", "Home");
                }

                ModelState.AddModelError("", "Invalid login credentials.");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult UserHome()
        {
            return View();
        }
    }
}
