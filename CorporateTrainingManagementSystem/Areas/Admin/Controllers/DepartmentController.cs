namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        public async Task<IActionResult> Index(int page=1, int pageSize=5, string? query=null, CancellationToken cancellationToken=default)
        {
            var departments = await _departmentService.GetAllAsync(page, pageSize, query, cancellationToken);

            return View(departments);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateDepartmentVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _departmentService.CreateAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message!);
                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
                return BadRequest();
            var department = await _departmentService.GetByIdAsync(id);

            if (department == null)
                return NotFound();

            var vm = department.Adapt<EditDepartmentVM>();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(EditDepartmentVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _departmentService.UpdateAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message!);
                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            var result = await _departmentService.DeleteAsync(id);

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
