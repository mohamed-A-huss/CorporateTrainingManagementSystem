
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Trainee.Controllers
{
    [Area(SD.TRAINEE_AREA)]
    [Authorize(Roles = SD.TRAINEE_ROLE)]

    public class BadgeController : Controller
    {
        private readonly ITraineeCourseService _courseService;

        public BadgeController(
            ITraineeCourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(
            CancellationToken cancellationToken)
        {
            var traineeId =
                User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var vm = await _courseService.GetMyBadgesAsync(
                traineeId,
                cancellationToken);

            return View(vm);
        }
    }
}
