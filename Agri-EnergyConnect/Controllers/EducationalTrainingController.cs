using Microsoft.AspNetCore.Mvc;

namespace Agri_EnergyConnect.Controllers
{
    public class EducationalTrainingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitResource(string name, string email, string link, string description)
        {
            TempData["Message"] = "Thanks for your submission!";
            return RedirectToAction("Index");
        }

    }
}
