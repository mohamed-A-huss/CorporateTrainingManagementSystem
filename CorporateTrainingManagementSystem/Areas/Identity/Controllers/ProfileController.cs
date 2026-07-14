using CorporateTrainingManagementSystem.Services.Interfaces;
using CorporateTrainingManagementSystem.ViewModels;
using CorporateTrainingManagementSystem.ViewModels.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CorporateTrainingManagementSystem.Areas.Identity.Controllers
{
    [Area(SD.IDENTITY_AREA)]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IAccountService _accountService;
        private string UserId =>User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public ProfileController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var vm = await _accountService.GetProfileAsync(
                UserId,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(CancellationToken cancellationToken)
        {
            var vm = await _accountService.GetEditProfileAsync(
                UserId,
                cancellationToken);

            if (vm is null)
                return NotFound();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result =
                await _accountService.UpdateProfileAsync(
                    UserId,
                    vm,
                    cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vm,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result =
                await _accountService.ChangePasswordAsync(
                    UserId,
                    vm,
                    cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                return View(vm);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
