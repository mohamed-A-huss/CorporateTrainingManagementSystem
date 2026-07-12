
namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var dashboardData = await _dashboardService.GetDashboardAsync(cancellationToken);
            return View(dashboardData);
        }
    }
}
