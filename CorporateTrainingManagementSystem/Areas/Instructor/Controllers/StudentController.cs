
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Instructor.Controllers
{
    [Area(SD.INSTRUCTOR_AREA)]
    [Authorize(Roles = SD.INSTRUCTOR_ROLE)]
    public class StudentController : Controller
    {
        private readonly IInstructorCourseService _courseService;

        public StudentController(
            IInstructorCourseService courseService)
        {
            _courseService = courseService;
        }
        public async Task<IActionResult> Index(
            CancellationToken cancellationToken)
        {
            var instructorId =
                User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var vm = await _courseService.GetInstructorStudentsAsync(
                instructorId,
                cancellationToken);

            return View(vm);
        }
    }
}
