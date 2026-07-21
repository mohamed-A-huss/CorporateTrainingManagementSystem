
using CorporateTrainingManagementSystem.Services.Implementations;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Trainee.Controllers
{
    [Area(SD.TRAINEE_AREA)]
    public class HomeController : Controller

    {
        private readonly ITraineeDashboardService _dashboardService;
        public HomeController(ITraineeDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        public async Task<IActionResult> Index()
        {
            

            var vm = await _dashboardService.GetDashboardAsync(UserId);

            return View(vm);
        }
    }
}
