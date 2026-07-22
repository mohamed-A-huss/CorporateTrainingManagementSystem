

using Microsoft.AspNetCore.Authorization;

namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 6,
            CourseFilter? filter = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _courseService.GetAllAsync(
                page,
                pageSize,
                filter,
                cancellationToken);
            ViewBag.Instructors = (await _courseService.GetCreateVMAsync()).Instructors;
            return View(result);
        }
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetByIdAsync(id);

            if (course == null)
                return NotFound();

            return View(course);
        }
        public async Task<IActionResult> Create()
        {
            var vm = await _courseService.GetCreateVMAsync();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await _courseService.GetCreateVMAsync();
                return View(vm);
            }

            var result = await _courseService.CreateAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message!);

                vm = await _courseService.GetCreateVMAsync();

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _courseService.GetEditVMAsync(id);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCourseVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Instructors = (await _courseService.GetCreateVMAsync()).Instructors;
                return View(vm);
            }

            var result = await _courseService.UpdateAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message!);

                vm.Instructors = (await _courseService.GetCreateVMAsync()).Instructors;

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseService.DeleteAsync(id);

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

    }
}
