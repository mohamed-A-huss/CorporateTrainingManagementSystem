using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Instructor.Controllers
{
    [Area(SD.INSTRUCTOR_AREA)]
    public class CourseController : Controller
    {
        private readonly IInstructorCourseService _courseService;
        private readonly IInstructorCourseManagementService _managementService;

        public CourseController(
            IInstructorCourseService courseService,
            IInstructorCourseManagementService managementService)
        {
            _courseService = courseService;
            _managementService = managementService;
        }

        private string UserId =>User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var courses = await _courseService.GetInstructorCoursesAsync(
                    UserId,
                    cancellationToken);

            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id,CancellationToken cancellationToken)
        {
            var course =
                await _courseService.GetDetailsAsync(
                    id,
                    UserId,
                    cancellationToken);

            if (course is null)
                return NotFound();

            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLesson(CreateLessonVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Details), new { id = vm.CourseId });
            }

            var result =
                await _managementService.CreateLessonAsync(
                    vm,
                    UserId,
                    cancellationToken);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Details), new { id = vm.CourseId });
        }
        [HttpGet]
        public async Task<IActionResult> EditLesson(int lessonId,CancellationToken cancellationToken)
        {
            var vm =
                await _managementService.GetLessonForEditAsync(
                    lessonId,
                    UserId,
                    cancellationToken);

            if (vm is null)
                return NotFound();

            return PartialView("_EditLessonModal", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLesson(InstructorEditLessonVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data.";

                return RedirectToAction(nameof(Details), new
                {
                    id = vm.CourseId
                });
            }
            Console.WriteLine($"LessonId = {vm.LessonId}");
            Console.WriteLine($"CourseId = {vm.CourseId}");
            var result =
                await _managementService.UpdateLessonAsync(
                    vm,
                    UserId,
                    cancellationToken);

            TempData[result.Success ? "Success" : "Error"] =
                result.Message;

            return RedirectToAction(nameof(Details), new
            {
                id = vm.CourseId
            });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLesson(int lessonId,int courseId,CancellationToken cancellationToken)
                {
            var result = await _managementService.DeleteLessonAsync(
                lessonId,
                UserId,
                cancellationToken);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Details), new
            {
                id = courseId
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExam(InstructorCreateExamVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid exam data.";

                return RedirectToAction(nameof(Details), new
                {
                    id = vm.CourseId
                });
            }

            var result = await _managementService.CreateExamAsync(
                vm,
                UserId,
                cancellationToken);

            TempData[result.Success ? "Success" : "Error"] =
                result.Message;

            return RedirectToAction(nameof(Details), new
            {
                id = vm.CourseId
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditExam(InstructorEditExamVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid exam data.";

                return RedirectToAction(nameof(Details), new
                {
                    id = vm.CourseId
                });
            }

            var result = await _managementService.UpdateExamAsync(
                vm,
                UserId,
                cancellationToken);

            TempData[result.Success ? "Success" : "Error"] =
                result.Message;

            return RedirectToAction(nameof(Details), new
            {
                id = vm.CourseId
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExam(int examId,int courseId,CancellationToken cancellationToken)
        {
            var result = await _managementService.DeleteExamAsync(
                examId,
                UserId,
                cancellationToken);

            TempData[result.Success ? "Success" : "Error"] =
                result.Message;

            return RedirectToAction(nameof(Details), new
            {
                id = courseId
            });
        }
    }
}
