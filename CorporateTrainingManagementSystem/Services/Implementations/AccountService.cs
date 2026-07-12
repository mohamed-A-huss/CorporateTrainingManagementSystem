using CorporateTrainingManagementSystem.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<ServiceResult> LoginAsync(LoginVM vm,CancellationToken cancellationToken = default)
        {
            // Check User

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user is null)
                return ServiceResult.Failure("Invalid email or password.");

            // Sign In

            var result = await _signInManager.PasswordSignInAsync(
                user,
                vm.Password,
                vm.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
                return ServiceResult.SuccessResult("Login successful.");

            if (result.IsLockedOut)
                return ServiceResult.Failure(
                    "Your account has been locked. Please try again later.");

            if (result.IsNotAllowed)
                return ServiceResult.Failure(
                    "Your account is not allowed to sign in.");

            return ServiceResult.Failure("Invalid email or password.");
        }
        public async Task<ServiceResult> RegisterAsync(RegisterVM vm,CancellationToken cancellationToken = default)
        {
            // Check Email
            var existingUser = await _userManager.FindByEmailAsync(vm.Email);

            if (existingUser is not null)
                return ServiceResult.Failure("Email is already registered.");

            var phoneExists = await _userManager.Users.AnyAsync(u => u.PhoneNumber == vm.PhoneNumber);

            if (phoneExists)
                return ServiceResult.Failure("Phone number is already registered.");

            

            // Create User

            var user = new ApplicationUser
            {
                FullName = vm.FullName,
                Email = vm.Email,
                UserName = vm.Email,
                PhoneNumber = vm.PhoneNumber
            };

            // Save User

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => e.Description));

                return ServiceResult.Failure(errors);
            }

            return ServiceResult.SuccessResult("Registration completed successfully.");
        }
        public Task<ServiceResult> ExternalLoginAsync(string provider, string returnUrl)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> ExternalLoginCallbackAsync(string? returnUrl, string? remoteError)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> ForgotPasswordAsync(ForgotPasswordVM vm, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ProfileVM?> GetProfileAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public Task<ServiceResult> ResetPasswordAsync(ResetPasswordVM vm, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateProfileAsync(EditProfileVM vm, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
