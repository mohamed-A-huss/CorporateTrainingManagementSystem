using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Instructor.Controllers
{
    [Area(SD.INSTRUCTOR_AREA)]
    public class HomeController : Controller
    {
        private readonly IInstructorDashboardService _dashboardService;

        public HomeController(IInstructorDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var vm = await _dashboardService.GetDashboardAsync(
                UserId,
                cancellationToken);

            return View(vm);
        }
    }
}
