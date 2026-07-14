using CorporateTrainingManagementSystem.Services.Implementations;
using CorporateTrainingManagementSystem.ViewModels.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CorporateTrainingManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IDepartmentService _departmentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateService _emailTemplateService;
        public UserController(IUserManagementService userManagementService, IDepartmentService departmentService, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IEmailTemplateService emailTemplateService)
        {
            _userManagementService = userManagementService;
            _departmentService = departmentService;
            _userManager = userManager;
            _emailSender = emailSender;
            _emailTemplateService = emailTemplateService;
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
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            ViewBag.Departments = new SelectList(
                (await _departmentService.GetAllAsync(
                    page: 1,
                    query: null,
                    pageSize: int.MaxValue,
                    cancellationToken: cancellationToken))
                .Departments,
                "DepartmentId",
                "Name");

            ViewBag.Roles = new SelectList(
                new[]
                {
            SD.ADMIN_ROLE,
            SD.INSTRUCTOR_ROLE
                });

            return View(new CreateUserVM());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM vm,CancellationToken cancellationToken)
        {
            if (vm.Role != SD.ADMIN_ROLE &&
                vm.Role != SD.INSTRUCTOR_ROLE)
            {
                ModelState.AddModelError(
                    nameof(vm.Role),
                    "Invalid role selected.");
            }

            if (!ModelState.IsValid)
            {
                await LoadCreateDataAsync(cancellationToken);

                return View(vm);
            }

            var result = await _userManagementService.CreateUserAsync(vm);


            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                await LoadCreateDataAsync(cancellationToken);

                return View(vm);
            }
            // Get Created User

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong.");

                await LoadCreateDataAsync(cancellationToken);

                return View(vm);
            }

            // Generate Confirmation Token

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Build Link

            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "Account",
                new
                {
                    area = "Identity",
                    userId = user.Id,
                    token
                },
                Request.Scheme);

            if (string.IsNullOrWhiteSpace(confirmationLink))
            {
                ModelState.AddModelError(string.Empty, "Unable to generate confirmation link.");

                await LoadCreateDataAsync(cancellationToken);

                return View(vm);
            }

            // Send Email

            await _emailSender.SendEmailAsync(
                user.Email!,
                "Confirm Your Email",
                _emailTemplateService.GetConfirmationEmailTemplate(confirmationLink));

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Index));
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
        private async Task LoadCreateDataAsync(CancellationToken cancellationToken)
        {
            ViewBag.Departments = new SelectList(
                (await _departmentService.GetAllAsync(
                    page: 1,
                    query: null,
                    pageSize: int.MaxValue,
                    cancellationToken: cancellationToken))
                .Departments,
                "DepartmentId",
                "Name");

            ViewBag.Roles = new SelectList(
                new[]
                {
            SD.ADMIN_ROLE,
            SD.INSTRUCTOR_ROLE
                });
        }
    }
}
