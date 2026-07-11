using CorporateTrainingManagementSystem.ViewModels.UserManagement;
using Microsoft.AspNetCore.Mvc;

namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        public UserController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6, UserFilter? filter = null,CancellationToken cancellationToken = default)
        {
            var vm = await _userManagementService.GetAllAsync(
                page: page,
                pageSize: pageSize,
                filter: filter,
                cancellationToken: cancellationToken);

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id,CancellationToken cancellationToken = default)
        {
            var vm = await _userManagementService.GetByIdAsync(
                id,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> ChangeRole(string id,CancellationToken cancellationToken)
        {
            var vm = await _userManagementService.GetChangeRoleVMAsync(
                id,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(ChangeRoleVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await _userManagementService.LoadRolesAsync(vm);

                return View(vm);
            }

            var result = await _userManagementService.ChangeRoleAsync(
                vm,
                cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                await _userManagementService.LoadRolesAsync(vm);

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLock(string id,CancellationToken cancellationToken)
        {
            var user = await _userManagementService.GetByIdAsync(
                id,
                cancellationToken);

            if (user is null)
                return NotFound();

            ServiceResult result;

            if (user.IsLocked)
            {
                result = await _userManagementService.UnlockAsync(
                    id,
                    cancellationToken);
            }
            else
            {
                result = await _userManagementService.LockAsync(
                    id,
                    cancellationToken);
            }

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
