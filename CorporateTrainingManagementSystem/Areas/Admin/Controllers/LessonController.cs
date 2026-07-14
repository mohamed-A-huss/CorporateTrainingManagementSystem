using CorporateTrainingManagementSystem.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LessonController : Controller
    {
        private readonly ILessonService _lessonService;
        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }
        public async Task<IActionResult> Index(int page = 1,int pageSize = 10,
            LessonFilter? filter = null,
            CancellationToken cancellationToken = default)
        {
            var lessons = await _lessonService.GetAllAsync(
                page,
                pageSize,
                filter,
                cancellationToken);
            ViewBag.Courses = (await _lessonService.GetCreateVMAsync()).Courses;

            return View(lessons);
        }
        public async Task<IActionResult> Details(int id)
        {
            var lesson = await _lessonService.GetByIdAsync(id);

            if (lesson is null)
                return NotFound();

            return View(lesson);
        }
        public async Task<IActionResult> Create()
        {
            var vm = await _lessonService.GetCreateVMAsync();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateLessonVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await _lessonService.LoadCoursesAsync(vm);

                return View(vm);
            }

            var result = await _lessonService.CreateAsync(
                vm,
                cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message!);

                await _lessonService.LoadCoursesAsync(vm);

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _lessonService.GetEditVMAsync(id);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            EditLessonVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await _lessonService.LoadCoursesAsync(vm);

                return View(vm);
            }

            var result = await _lessonService.UpdateAsync(
                vm,
                cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message!);

                await _lessonService.LoadCoursesAsync(vm);

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
            var result = await _lessonService.DeleteAsync(
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
