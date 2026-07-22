using Microsoft.AspNetCore.Authorization;


namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6, EnrollmentFilter? filter = null,CancellationToken cancellationToken = default)
        {
            var vm = await _enrollmentService.GetAllAsync(page,pageSize,filter,cancellationToken);

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id,CancellationToken cancellationToken)
        {
            var vm = await _enrollmentService.GetByIdAsync(
                id,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var vm = new CreateEnrollmentVM();

            await _enrollmentService.LoadDropdownsAsync(
                vm,
                cancellationToken);

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEnrollmentVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await _enrollmentService.LoadDropdownsAsync(
                    vm,
                    cancellationToken);

                return View(vm);
            }

            var result = await _enrollmentService.CreateAsync(
                vm,
                cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                await _enrollmentService.LoadDropdownsAsync(
                    vm,
                    cancellationToken);

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id,EnrollmentStatus status,CancellationToken cancellationToken)
        {
            var result = await _enrollmentService.ChangeStatusAsync(
                id,
                status,
                cancellationToken);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var result = await _enrollmentService.DeleteAsync(
                id,
                cancellationToken);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
