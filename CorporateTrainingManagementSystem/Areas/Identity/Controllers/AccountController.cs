
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace CorporateTrainingManagementSystem.Areas.Identity.Controllers
{
    [Area(SD.IDENTITY_AREA)]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IDepartmentService _departmentService;

        public AccountController(
            IAccountService accountService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IEmailTemplateService emailTemplateService,
            IDepartmentService departmentService)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _emailTemplateService = emailTemplateService;
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var vm = new RegisterVM
            {
                Departments = (await _departmentService.GetDropdownAsync())
                    .Select(d => new SelectListItem
                    {
                        Value = d.DepartmentId.ToString(),
                        Text = d.Name
                    })
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Departments = (await _departmentService.GetDropdownAsync())
                    .Select(d => new SelectListItem
                    {
                        Value = d.DepartmentId.ToString(),
                        Text = d.Name
                    });

                return View(vm);
            }

            var result = await _accountService.RegisterAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(vm);
            }

            // Get Created User

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong.");
                return View(vm);
            }

            // Generate Email Confirmation Token

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Build Confirmation Link

            var confirmationLink = Url.Action(
                nameof(ConfirmEmail),
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
                return View(vm);
            }

            // Send Email

            await _emailSender.SendEmailAsync(
                user.Email!,
                "Confirm Your Email",
                _emailTemplateService.GetConfirmationEmailTemplate(confirmationLink));

            TempData["Success"] =
                "Registration completed successfully. Please check your email to activate your account.";

            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(token))
            {
                TempData["Error"] = "Invalid confirmation link.";

                return RedirectToAction(nameof(Login));
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                TempData["Error"] = "User not found.";

                return RedirectToAction(nameof(Login));
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Email confirmation failed.";

                return RedirectToAction(nameof(Login));
            }

            TempData["Success"] =
                "Your email has been confirmed successfully. You can now log in.";

            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _accountService.LoginAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                return View(vm);
            }

            var user = await _userManager.FindByEmailAsync(vm.Email);
            TempData["Success"] = "Welcome back.";

            return await RedirectToDashboard(user);
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _accountService.ForgotPasswordAsync(vm);

            if (!result.Result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Result.Message);
                return View(vm);
            }

            // Generate Reset Password Link

            var resetPasswordLink = Url.Action(
                nameof(ResetPassword),
                "Account",
                new
                {
                    area = "Identity",
                    email = result.User!.Email,
                    token = result.Token
                },
                Request.Scheme);

            if (string.IsNullOrWhiteSpace(resetPasswordLink))
            {
                ModelState.AddModelError(string.Empty, "Unable to generate reset password link.");
                return View(vm);
            }

            // Send Email

            await _emailSender.SendEmailAsync(
                result.User.Email!,
                "Reset Your Password",
                _emailTemplateService.GetResetPasswordTemplate(resetPasswordLink));

            TempData["Success"] =
                "Password reset link has been sent to your email.";

            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(token))
            {
                TempData["Error"] = "Invalid password reset link.";

                return RedirectToAction(nameof(Login));
            }

            var vm = new ResetPasswordVM
            {
                Email = email,
                Token = token
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _accountService.ResetPasswordAsync(vm);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                return View(vm);
            }

            TempData["Success"] =
                "Your password has been reset successfully. You can now log in.";

            return RedirectToAction(nameof(Login));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider,string? returnUrl = null)
        {
            var redirectUrl = Url.Action(
                nameof(ExternalLoginCallback),
                "Account",
                new
                {
                    returnUrl
                });

            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(
                    provider,
                    redirectUrl);

            return Challenge(properties, provider);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null,string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!string.IsNullOrWhiteSpace(remoteError))
            {
                TempData["Error"] = remoteError;
                return RedirectToAction(nameof(Login));
            }

            // Get External Login Info

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info is null)
            {
                TempData["Error"] = "Unable to load external login information.";
                return RedirectToAction(nameof(Login));
            }

            // Try Sign In

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);


            if (signInResult.Succeeded) {
                var user1 = await _userManager.FindByLoginAsync(info.LoginProvider,info.ProviderKey);

                return await RedirectToDashboard(user1!);
            }

            // Read Email

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Email address was not returned from Google.";
                return RedirectToAction(nameof(Login));
            }

            // Check Existing User

            var user = await _userManager.FindByEmailAsync(email);

            if (user is not null)
            {
                // Link Google Account

                //var addLoginResult = await _userManager.AddLoginAsync(user, info);

                //if (!addLoginResult.Succeeded)
                //{
                //    TempData["Error"] = string.Join(
                //        Environment.NewLine,
                //        addLoginResult.Errors.Select(e => e.Description));

                //    return RedirectToAction(nameof(Login));
                //}
                var logins = await _userManager.GetLoginsAsync(user);

                if (!logins.Any(x =>
                    x.LoginProvider == info.LoginProvider &&
                    x.ProviderKey == info.ProviderKey))
                {
                    var addLoginResult =
                        await _userManager.AddLoginAsync(user, info);

                    if (!addLoginResult.Succeeded)
                    {
                        TempData["Error"] = string.Join(    
                            Environment.NewLine,
                            addLoginResult.Errors.Select(e => e.Description));

                        return RedirectToAction(nameof(Login));
                    }
                }
            }
            else
            {
                // Create New Trainee

                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName =
                        info.Principal.FindFirstValue(ClaimTypes.Name) ??
                        email,
                    EmailConfirmed = true,
                    DepartmentId = SD.GENERAL_DEPARTMENT_ID
                };

                var createResult =
                    await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    TempData["Error"] = string.Join(
                        Environment.NewLine,
                        createResult.Errors.Select(e => e.Description));

                    return RedirectToAction(nameof(Login));
                }

                // Assign Role

                await _userManager.AddToRoleAsync(
                    user,
                    SD.TRAINEE_ROLE);

                // Link Google Login

                var addLoginResult =
                    await _userManager.AddLoginAsync(user, info);

                if (!addLoginResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);

                    TempData["Error"] = string.Join(
                        Environment.NewLine,
                        addLoginResult.Errors.Select(e => e.Description));

                    return RedirectToAction(nameof(Login));
                }
            }

            // Sign In

            await _signInManager.SignInAsync(
                user,
                isPersistent: false);

            return await RedirectToDashboard(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData["Success"] = "Logged out successfully.";

            return RedirectToAction(nameof(Login));
        }
        public IActionResult Error404()
        {
            return View();
        }
        private async Task<IActionResult> RedirectToDashboard(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains(SD.SUPER_ADMIN_ROLE) ||
                roles.Contains(SD.ADMIN_ROLE))
            {
                return RedirectToAction(
                    "Index",
                    "Home",
                    new { area = SD.ADMIN_AREA });
            }

            if (roles.Contains(SD.INSTRUCTOR_ROLE))
            {
                return RedirectToAction(
                    "Index",
                    "Home",
                    new { area = SD.INSTRUCTOR_AREA });
            }

            if (roles.Contains(SD.TRAINEE_ROLE))
            {
                return RedirectToAction(
                    "Index",
                    "Home",
                    new { area = SD.TRAINEE_AREA });
            }

            await _signInManager.SignOutAsync();

            TempData["Error"] = "No role has been assigned to your account.";
                
            return RedirectToAction(nameof(Login));
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
