
using Microsoft.AspNetCore.Authorization;

namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
    public class QuestionController : Controller
    { private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            this._questionService = questionService;
        }
        public async Task<IActionResult> Index(QuestionFilter? filter,int page = 1,CancellationToken cancellationToken = default)
        {
            var vm = await _questionService.GetAllAsync(
                page: page,
                filter: filter,
                cancellationToken: cancellationToken);

            return View(vm);
        }
        public async Task<IActionResult> Details(int id,CancellationToken cancellationToken = default)
        {
            var vm = await _questionService.GetByIdAsync(id);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        public async Task<IActionResult> Create()
        {
            var vm = await _questionService.GetCreateVMAsync();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuestionVM vm,CancellationToken cancellationToken)
        {
            if (vm.QuestionType == QuestionType.TrueFalse)
            {
                for (int i = 0; i < vm.Choices.Count; i++)
                {
                    ModelState.Remove($"Choices[{i}].ChoiceText");
                }
            }
            if (!ModelState.IsValid)
            {
                await _questionService.LoadDropdownsAsync(vm);
                return View(vm);
            }

            var result = await _questionService.CreateAsync(
                vm,
                cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                await _questionService.LoadDropdownsAsync(vm);

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id,CancellationToken cancellationToken)
        {
            var vm = await _questionService.GetEditVMAsync(id,cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditQuestionVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await _questionService.LoadDropdownsAsync(vm);
                return View(vm);
            }

            var result = await _questionService.UpdateAsync(
                vm,
                cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                await _questionService.LoadDropdownsAsync(vm);

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var result = await _questionService.DeleteAsync(
                id,
                cancellationToken);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> GetExams(int courseId)
        {
            var exams = await _questionService.GetExamsByCourseAsync(courseId);

            return Json(exams);
        }
    }
}
