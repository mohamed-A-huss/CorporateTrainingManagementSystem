    
namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BadgeController : Controller
    {
        private readonly IBadgeService _badgeService;
        public BadgeController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }  
        public async Task<IActionResult> Index(BadgeFilter filter, int page = 1, int pageSize = 10, CancellationToken cancellationToken=default)
        {
            var badges = await _badgeService.GetAllAsync(filter, page, pageSize, cancellationToken);

            return View(badges);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBadgeVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _badgeService.CreateAsync(vm);

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
            var badge = await _badgeService.GetByIdAsync(id);

            if (badge == null)
                return NotFound();

            var vm = badge.Adapt<EditBadgeVM>();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBadgeVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _badgeService.UpdateAsync(vm);

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
            var result = await _badgeService.DeleteAsync(id);

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
