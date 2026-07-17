
namespace CorporateTrainingManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(
    "Login",
    "Account",
    new { area = SD.IDENTITY_AREA });
        }
    }
}
