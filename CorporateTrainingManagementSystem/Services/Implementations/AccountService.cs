using CorporateTrainingManagementSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IFileService _fileService;

        public AccountService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IEmailSender emailSender,IFileService fileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _fileService = fileService;
        }

        public async Task<ServiceResult> LoginAsync(LoginVM vm,CancellationToken cancellationToken = default)
        {
            // Check User

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user is null)
                return ServiceResult.Failure("Invalid email or password.");

            // Check Email Confirmation

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return ServiceResult.Failure(
                    "Please confirm your email before logging in.");
            }

            // Sign In

            var result = await _signInManager.PasswordSignInAsync(
                user,
                vm.Password,
                vm.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
                return ServiceResult.SuccessResult("Welcome back.");

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

            // Check Phone Number

            bool phoneExists = await _userManager.Users
                .AnyAsync(u => u.PhoneNumber == vm.PhoneNumber, cancellationToken);

            if (phoneExists)
                return ServiceResult.Failure("Phone number is already registered.");

            // Create User

            var user = new ApplicationUser
            {
                FullName = vm.FullName,
                UserName = vm.Email,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                DepartmentId = vm.DepartmentId
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
            await _userManager.AddToRoleAsync(user,SD.TRAINEE_ROLE);
            return ServiceResult.SuccessResult(
                "Registration completed successfully.");
        }
        

        public async Task<ForgotPasswordResult> ForgotPasswordAsync(ForgotPasswordVM vm,CancellationToken cancellationToken = default)
        {
            var result = new ForgotPasswordResult();

            // Find User

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user is null)
            {
                result.Result = ServiceResult.Failure(
                    "No account found with this email.");

                return result;
            }

            // Check Email Confirmation

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                result.Result = ServiceResult.Failure(
                    "Please confirm your email first.");

                return result;
            }

            // Generate Reset Password Token

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            result.User = user;
            result.Token = token;

            result.Result = ServiceResult.SuccessResult(
                "Password reset token generated successfully.");

            return result;
        }
        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordVM vm,CancellationToken cancellationToken = default)
        {
            // Find User

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user is null)
                return ServiceResult.Failure("User not found.");

            // Reset Password

            var result = await _userManager.ResetPasswordAsync(
                user,
                vm.Token,
                vm.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => e.Description));

                return ServiceResult.Failure(errors);
            }

            return ServiceResult.SuccessResult(
                "Password has been reset successfully.");
        }


        public async Task<ServiceResult> ChangePasswordAsync(string userId,ChangePasswordVM vm,CancellationToken cancellationToken = default)
        {
            // Check User

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return ServiceResult.Failure("User not found.");

            // Change Password

            var result = await _userManager.ChangePasswordAsync(
                user,
                vm.CurrentPassword,
                vm.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => e.Description));

                return ServiceResult.Failure(errors);
            }

            // Refresh SignIn Cookie

            await _signInManager.RefreshSignInAsync(user);

            return ServiceResult.SuccessResult(
                "Password changed successfully.");
        }
        public async Task<ProfileVM?> GetProfileAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .Include(u => u.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.Id == userId,
                    cancellationToken);

            if (user is null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new ProfileVM
            {
                FullName = user.FullName,

                Email = user.Email!,

                PhoneNumber = user.PhoneNumber ?? string.Empty,

                ProfilePicture = user.ProfilePicture,

                Department = user.Department?.Name ?? "No Department",

                Role = roles.FirstOrDefault() ?? "No Role",

                EmailConfirmed = user.EmailConfirmed
            };
        }
        public async Task<EditProfileVM?> GetEditProfileAsync(string userId,CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.Id == userId,
                    cancellationToken);

            if (user is null)
                return null;

            return new EditProfileVM
            {
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                CurrentProfilePicture = user.ProfilePicture
            };
        }
        public async Task<ServiceResult> UpdateProfileAsync(string userId,EditProfileVM vm,CancellationToken cancellationToken = default)
        {
            // Find User

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return ServiceResult.Failure("User not found.");

            // Check Email

            var emailExists = await _userManager.Users
                .AnyAsync(u =>
                    u.Email == vm.Email &&
                    u.Id != userId,
                    cancellationToken);

            if (emailExists)
                return ServiceResult.Failure("Email is already registered.");

            // Check Phone Number

            var phoneExists = await _userManager.Users
                .AnyAsync(u =>
                    u.PhoneNumber == vm.PhoneNumber &&
                    u.Id != userId,
                    cancellationToken);

            if (phoneExists)
                return ServiceResult.Failure("Phone number is already registered.");
            if (vm.ProfilePicture is not null)
            {
                var uploadResult = await _fileService.UploadFileAsync(
                    vm.ProfilePicture,
                    UploadFolders.Profiles,
                    cancellationToken);

                if (!uploadResult.Success)
                    return ServiceResult.Failure(uploadResult.ErrorMessage);

                _fileService.DeleteFile(user.ProfilePicture);

                user.ProfilePicture = uploadResult.FilePath;
            }
            // Update User

            user.FullName = vm.FullName;
            user.Email = vm.Email;
            user.UserName = vm.Email;
            user.PhoneNumber = vm.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => e.Description));

                return ServiceResult.Failure(errors);
            }

            return ServiceResult.SuccessResult(
                "Profile updated successfully.");
        }
    }
}
