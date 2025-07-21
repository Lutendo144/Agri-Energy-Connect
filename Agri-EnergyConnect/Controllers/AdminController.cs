using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Agri_EnergyConnect.Models;
using Agri_EnergyConnect.Models.ViewModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agri_EnergyConnect.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _connectionString = "Server=tcp:eventease6.database.windows.net,1433;Initial Catalog=AgriEnergyConnect;Persist Security Info=False;User ID=eventEase;Password=GreatCopy@2024;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


      // admin users
        private readonly List<Admin> _admins = new List<Admin>
        {
            new Admin { FullName = "Admin One", Email = "admin1@admin.com", Password = "Admin@123" },
            new Admin { FullName = "Admin Two", Email = "admin2@admin.com", Password = "Easy@456" },
            new Admin { FullName = "Admin Three", Email = "admin3@admin.com", Password = "Strong@789" }
        };

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = _admins.FirstOrDefault(a =>
                a.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase) &&
                a.Password == model.Password);

            if (admin != null)
            {
                HttpContext.Session.SetString("AdminEmail", admin.Email);
                HttpContext.Session.SetString("AdminName", admin.FullName);
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
                return RedirectToAction("Login");

            ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        //ManageUsers
        public async Task<IActionResult> ManageUser()
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
                return RedirectToAction("Login");

            var users = new List<AppUser>();

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();
            string query = "SELECT Id, FullName, Email, PhoneNumber, Password, Role FROM AppUsers";

            using SqlCommand cmd = new(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new AppUser
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    PhoneNumber = reader.GetString(3),
                    Password = reader.GetString(4),
                    Role = reader.GetString(5)
                });
            }

            return View(users);
        }

        //Add user (GET)
        public IActionResult CreateUser()
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
                return RedirectToAction("Login");

            return View();
        }

        // Add user (POST)
        [HttpPost]
        public async Task<IActionResult> CreateUser(AppUser user)
        {
            if (!ModelState.IsValid)
                return View(user);

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string insert = @"INSERT INTO AppUsers (FullName, Email, PhoneNumber, Password, Role) 
                              VALUES (@FullName, @Email, @PhoneNumber, @Password, @Role)";

            using SqlCommand cmd = new(insert, conn);
            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Role", user.Role);

            await cmd.ExecuteNonQueryAsync();
            return RedirectToAction("ManageUser");
        }


        public async Task<IActionResult> ManageMarketPlace(string category, DateTime? startDate, DateTime? endDate)
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
                return RedirectToAction("Login");

            var products = new List<MarketplaceItem>();

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            var query = "SELECT * FROM MarketplaceItems WHERE 1=1";
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(category))
            {
                query += " AND Category LIKE @Category";
                parameters.Add(new SqlParameter("@Category", $"%{category}%"));
            }

            if (startDate.HasValue)
            {
                query += " AND DateAdded >= @StartDate";
                parameters.Add(new SqlParameter("@StartDate", startDate.Value));
            }

            if (endDate.HasValue)
            {
                query += " AND DateAdded <= @EndDate";
                parameters.Add(new SqlParameter("@EndDate", endDate.Value));
            }

            query += " ORDER BY DateAdded DESC";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddRange(parameters.ToArray());

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                products.Add(new MarketplaceItem
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    Category = reader.GetString(reader.GetOrdinal("Category")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                    SellerName = reader.IsDBNull(reader.GetOrdinal("SellerName")) ? null : reader.GetString(reader.GetOrdinal("SellerName")),
                    SellerEmail = reader.IsDBNull(reader.GetOrdinal("SellerEmail")) ? null : reader.GetString(reader.GetOrdinal("SellerEmail")),
                    DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded"))
                });
            }

            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string email)
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
                return RedirectToAction("Login");

            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required");

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string query = "DELETE FROM AppUsers WHERE Email = @Email";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            await cmd.ExecuteNonQueryAsync();

            return RedirectToAction("ManageUser");
        }

        // EditUser
        public async Task<IActionResult> EditUser(string email)
        {
            if (HttpContext.Session.GetString("AdminEmail") == null)
                return RedirectToAction("Login");

            if (string.IsNullOrEmpty(email))
                return NotFound();

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            string query = "SELECT Id, FullName, Email, PhoneNumber, Password, Role FROM AppUsers WHERE Email = @Email";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var user = new AppUser
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    PhoneNumber = reader.GetString(3),
                    Password = reader.GetString(4),
                    Role = reader.GetString(5)
                };

                return View(user); 
            }

            return NotFound();
        }


    }
}
