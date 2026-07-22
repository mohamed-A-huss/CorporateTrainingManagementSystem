
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Trainee.Controllers
{
    [Area(SD.TRAINEE_AREA)]
    [Authorize(Roles = SD.TRAINEE_ROLE)]

    public class ExamController : Controller
    {
        private readonly ITraineeCourseService _courseService;

        public ExamController(ITraineeCourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Start(
            int id,
            CancellationToken cancellationToken)
        {
            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var vm = await _courseService.GetExamStartAsync(
                id,
                traineeId,
                cancellationToken);

            if (vm is null)
                return NotFound();

            if (!vm.CanStart)
            {
                TempData["Error"] =
                    "You cannot start this exam. Complete all lessons and make sure you still have remaining attempts.";

                return RedirectToAction(
                    "Details",
                    "Course",
                    new
                    {
                        area = SD.TRAINEE_AREA,
                        id = vm.CourseId
                    });
            }

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Begin(
                int examId,
                CancellationToken cancellationToken)
        {
            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var result = await _courseService.BeginExamAsync(
                examId,
                traineeId,
                cancellationToken);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                return RedirectToAction(nameof(Start), new { id = examId });
            }

            return RedirectToAction(
                nameof(Take),
                new
                {
                    attemptId = result.Data
                });
        }
        public async Task<IActionResult> Take(
                int attemptId,
                CancellationToken cancellationToken)
        {
            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var vm = await _courseService.GetTakeExamAsync(
                attemptId,
                traineeId,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finish(SubmitExamVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please answer all required questions.";

                return RedirectToAction(nameof(Take), new
                {
                    attemptId = vm.AttemptId
                });
            }

            var traineeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var answers = vm.Answers.ToDictionary(
                q => q.QuestionId,
                q => q.SelectedChoiceId);

            var result = await _courseService.FinishExamAsync(
                vm.AttemptId,
                traineeId,
                answers,
                cancellationToken);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                return RedirectToAction(
                    nameof(Take),
                    new
                    {
                        attemptId = vm.AttemptId
                    });
            }

            return View("Result", result.Data);
        }
        

    }
}
