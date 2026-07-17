using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Instructor.Controllers
{
    [Area(SD.INSTRUCTOR_AREA)]
    public class ExamController : Controller
    {
        private readonly IInstructorExamService _examService;

        public ExamController(IInstructorExamService examService)
        {
            _examService = examService;
        }

        private string UserId =>User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        public async Task<IActionResult> Details(int id,CancellationToken cancellationToken)
        {
            var vm = await _examService.GetDetailsAsync(
                id,
                UserId,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(InstructorCreateQuestionVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data.";

                return RedirectToAction(nameof(Details), new
                {
                    id = vm.ExamId
                });
            }

            var result = await _examService.CreateQuestionAsync(
                vm,
                UserId,
                cancellationToken);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Details), new
            {
                id = vm.ExamId
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(InstructorEditQuestionVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data.";

                return RedirectToAction(nameof(Details), new
                {
                    id = vm.ExamId
                });
            }

            var result = await _examService.UpdateQuestionAsync(
                vm,
                UserId,
                cancellationToken);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Details), new
            {
                id = vm.ExamId
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion(int questionId,int examId,CancellationToken cancellationToken)
        {
            var result = await _examService.DeleteQuestionAsync(
                questionId,
                UserId,
                cancellationToken);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Details), new
            {
                id = examId
            });
        }
        public async Task<IActionResult> QuestionDetails(int id,CancellationToken cancellationToken)
        {
            var vm = await _examService.GetQuestionDetailsAsync(
                id,
                UserId,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChoice(InstructorCreateChoiceVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data.";

                return RedirectToAction(
                    nameof(QuestionDetails),
                    new { id = vm.QuestionId });
            }

            var result = await _examService.CreateChoiceAsync(
                vm,
                UserId,
                cancellationToken);

            TempData[result.Success ? "Success" : "Error"] =
                result.Message;

            return RedirectToAction(
                nameof(QuestionDetails),
                new { id = vm.QuestionId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditChoice(InstructorEditChoiceVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data.";

                return RedirectToAction(
                    nameof(QuestionDetails),
                    new { id = vm.QuestionId });
            }

            var result = await _examService.UpdateChoiceAsync(
                vm,
                UserId,
                cancellationToken);

            TempData[result.Success ? "Success" : "Error"] =
                result.Message;

            return RedirectToAction(
                nameof(QuestionDetails),
                new { id = vm.QuestionId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteChoice(int choiceId,int questionId,CancellationToken cancellationToken)
        {
            var result = await _examService.DeleteChoiceAsync(
                choiceId,
                UserId,
                cancellationToken);

            TempData[result.Success ? "Success" : "Error"] =
                result.Message;

            return RedirectToAction(
                nameof(QuestionDetails),
                new { id = questionId });
        }
    }
}
