using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Instructor.Controllers
{
    [Area(SD.INSTRUCTOR_AREA)]
    [Authorize(Roles = SD.INSTRUCTOR_ROLE)]

    public class HomeController : Controller
    {
        private readonly IInstructorDashboardService _dashboardService;

        public HomeController(IInstructorDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var vm = await _dashboardService.GetDashboardAsync(
                userId,
                cancellationToken);

            return View(vm);
        }
    }
}
