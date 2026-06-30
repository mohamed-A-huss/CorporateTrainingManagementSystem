using Microsoft.AspNetCore.Mvc;

namespace CorporateTrainingManagementSystem.Areas.Identity.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
