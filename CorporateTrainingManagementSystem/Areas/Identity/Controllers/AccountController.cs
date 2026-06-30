using Microsoft.AspNetCore.Mvc;

namespace CorporateTrainingManagementSystem.Areas.Identity.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
