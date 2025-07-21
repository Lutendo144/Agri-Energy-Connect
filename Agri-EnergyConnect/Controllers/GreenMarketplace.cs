using Microsoft.AspNetCore.Mvc;
using Agri_EnergyConnect.Models;
using Agri_EnergyConnect.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace Agri_EnergyConnect.Controllers
{
    public class GreenMarketplaceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GreenMarketplaceController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

   
        public IActionResult AddItem()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(MarketplaceItem model, IFormFile imageFile)
        {
            ModelState.Remove("ImageUrl");
            ModelState.Remove("SellerName");
            ModelState.Remove("SellerEmail");

            if (ModelState.IsValid)
            {
               
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder); 

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    model.ImageUrl = "/images/" + uniqueFileName;
                }
                else
                {
                    model.ImageUrl = "/images/default-placeholder.png"; 
                }

               
                model.SellerName = "Anonymous";
                model.SellerEmail = "example@email.com"; 

                model.DateAdded = DateTime.Now;

                _context.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Item successfully added to the marketplace!";
                return RedirectToAction("Index");
            }

            return View(model);
        }



       
        public async Task<IActionResult> Index()
        {
            var items = await _context.MarketplaceItems
                .OrderByDescending(m => m.DateAdded)
                .ToListAsync();

            return View(items);
        }
    }
}
