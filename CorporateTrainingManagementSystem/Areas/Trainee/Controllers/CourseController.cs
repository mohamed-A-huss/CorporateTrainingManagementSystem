using CorporateTrainingManagementSystem.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Trainee.Controllers
{
    [Area(SD.TRAINEE_AREA)]
    public class CourseController : Controller
    {
        private readonly ITraineeCourseService _courseService;

        public CourseController(
            ITraineeCourseService courseService)
        {
            _courseService = courseService;
        }
        public async Task<IActionResult> MyCourses(CancellationToken cancellationToken)
        {
            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var vm = await _courseService.GetMyCoursesAsync(
                traineeId,
                cancellationToken);

            return View(vm);
        }
        public async Task<IActionResult> Details(int id,CancellationToken cancellationToken)
        {
            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var vm = await _courseService.GetCourseDetailsAsync(
                id,
                traineeId,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        public async Task<IActionResult> Lesson(int id,CancellationToken cancellationToken)
        {
            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var vm = await _courseService.GetLessonDetailsAsync(
                id,
                traineeId,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int lessonId,CancellationToken cancellationToken)
        {
            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await _courseService.MarkLessonAsReadAsync(
                lessonId,
                traineeId,
                cancellationToken);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = result.Message;
            }

            return RedirectToAction(nameof(Lesson), new { id = lessonId });
        }
        public async Task<IActionResult> Browse(CancellationToken cancellationToken)
        {
            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var vm = await _courseService.GetBrowseCoursesAsync(
                traineeId,
                cancellationToken);

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int courseId,   CancellationToken cancellationToken)
        {
            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await _courseService.EnrollAsync(
                courseId,
                traineeId,
                cancellationToken);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Browse));
        }
        public async Task<IActionResult> Preview(int id,CancellationToken cancellationToken)
        {
            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var vm = await _courseService.GetPublicCourseDetailsAsync(
                id,
                traineeId,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }

    }
}
