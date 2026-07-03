using Microsoft.AspNetCore.Mvc;

namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
