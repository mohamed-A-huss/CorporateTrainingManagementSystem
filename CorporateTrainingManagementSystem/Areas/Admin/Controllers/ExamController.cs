using Microsoft.AspNetCore.Authorization;

namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
    public class ExamController : Controller
    {
        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 10,
            ExamFilter? filter = null,
            CancellationToken cancellationToken = default)
        {
            var exams = await _examService.GetAllAsync(
                page,
                pageSize,
                filter,
                cancellationToken);

            ViewBag.Courses = (await _examService.GetCreateVMAsync()).Courses;

            return View(exams);
        }

        public async Task<IActionResult> Details(int id)
        {
            var exam = await _examService.GetByIdAsync(id);

            if (exam is null)
                return NotFound();

            return View(exam);
        }

        public async Task<IActionResult> Create()
        {
            var vm = await _examService.GetCreateVMAsync();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateExamVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await _examService.LoadCoursesAsync(vm);

                return View(vm);
            }

            var result = await _examService.CreateAsync(
                vm,
                cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message!);

                await _examService.LoadCoursesAsync(vm);

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _examService.GetEditVMAsync(id);

            if (vm is null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            EditExamVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await _examService.LoadCoursesAsync(vm);

                return View(vm);
            }

            var result = await _examService.UpdateAsync(
                vm,
                cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message!);

                await _examService.LoadCoursesAsync(vm);

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _examService.DeleteAsync(
                id,
                cancellationToken);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;

                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}