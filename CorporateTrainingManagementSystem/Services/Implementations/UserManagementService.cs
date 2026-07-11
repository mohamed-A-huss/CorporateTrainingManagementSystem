using CorporateTrainingManagementSystem.DataAccess;
using CorporateTrainingManagementSystem.ViewModels.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace CorporateTrainingManagementSystem.Services.Implementations

{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;


        public UserManagementService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<ServiceResult> ChangeRoleAsync(ChangeRoleVM vm,CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(vm.UserId);

            if (user is null)
                return ServiceResult.Failure("User not found.");

            if (!await _roleManager.RoleExistsAsync(vm.SelectedRole))
                return ServiceResult.Failure("Selected role does not exist.");

            var currentRole = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(
                    _context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => r.Name)
                .FirstOrDefaultAsync(cancellationToken);

            if (currentRole == vm.SelectedRole)
                return ServiceResult.Failure("User already has this role.");

            if (!string.IsNullOrWhiteSpace(currentRole))
            {
                var removeResult = await _userManager.RemoveFromRoleAsync(
                    user,
                    currentRole);

                if (!removeResult.Succeeded)
                {
                    return ServiceResult.Failure(
                        string.Join(Environment.NewLine,
                            removeResult.Errors.Select(e => e.Description)));
                }
            }

            var addResult = await _userManager.AddToRoleAsync(
                user,
                vm.SelectedRole);

            if (!addResult.Succeeded)
            {
                return ServiceResult.Failure(
                    string.Join(Environment.NewLine,
                        addResult.Errors.Select(e => e.Description)));
            }

            return ServiceResult.SuccessResult("User role updated successfully.");
        }

        public async Task<PaginatedUsers> GetAllAsync(int page = 1,int pageSize = 10,UserFilter? filter = null,CancellationToken cancellationToken = default)
        {
            filter ??= new UserFilter();

            await LoadDropdownsAsync(filter, cancellationToken);

            var users = await _context.Users
                .Include(u => u.Department)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var userRoles = await (
                from ur in _context.UserRoles
                join r in _context.Roles
                    on ur.RoleId equals r.Id
                select new
                {
                    ur.UserId,
                    Role = r.Name!
                })
                .ToListAsync(cancellationToken);

            var result = users.Select(user =>
            {
                var role = userRoles
                    .FirstOrDefault(x => x.UserId == user.Id)?.Role ?? string.Empty;

                return new UserVM
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email ?? string.Empty,
                    DepartmentId = user.DepartmentId,
                    Department = user.Department?.Name ?? string.Empty,
                    Role = role,
                    Points = user.Points,
                    LockoutEnd = user.LockoutEnd,
                    IsLocked = user.LockoutEnd.HasValue &&
                               user.LockoutEnd > DateTimeOffset.UtcNow
                };
            });

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim();

                result = result.Where(u =>
                    u.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.DepartmentId.HasValue)
            {
                result = result.Where(u =>
                    u.DepartmentId == filter.DepartmentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Role))
            {
                result = result.Where(u =>
                    u.Role == filter.Role);
            }

            var totalCount = result.Count();

            var items = result
                .OrderBy(u => u.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedUsers
            {
                Users = items,
                CurrentPage = page,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Filter = filter
            };
        }
        public async Task<UserDetailsVM?> GetByIdAsync(string id,CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .Include(u => u.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.Id == id,
                    cancellationToken);

            if (user is null)
                return null;

            var role = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(
                    _context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => r.Name)
                .FirstOrDefaultAsync(cancellationToken);

            return new UserDetailsVM
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Department = user.Department?.Name ?? string.Empty,
                Role = role ?? string.Empty,
                Points = user.Points,
                EmailConfirmed = user.EmailConfirmed,
                IsLocked = user.LockoutEnd.HasValue &&
                           user.LockoutEnd > DateTimeOffset.UtcNow
            };
        }

        public async Task<ChangeRoleVM?> GetChangeRoleVMAsync(string id,CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.Id == id,
                    cancellationToken);

            if (user is null)
                return null;

            var currentRole = await _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(
                    _context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => r.Name)
                .FirstOrDefaultAsync(cancellationToken);
            if (currentRole == SD.ADMIN_ROLE ||currentRole == SD.SUPER_ADMIN_ROLE)
            {
                return null;
            }
            var vm = new ChangeRoleVM
            {
                UserId = user.Id,
                FullName = user.FullName,
                SelectedRole = currentRole ?? string.Empty
            };

            await LoadRolesAsync(vm);

            return vm;
        }

        public async Task LoadDropdownsAsync(UserFilter filter,CancellationToken cancellationToken = default)
        {
            filter.Departments = (await _unitOfWork.Departments.GetAsync(
                tracked: false,
                cancellationToken: cancellationToken))
                .OrderBy(d => d.Name)
                .Select(d => new SelectListItem
                {
                    Value = d.DepartmentId.ToString(),
                    Text = d.Name
                });

            filter.Roles = await _roleManager.Roles
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Value = r.Name!,
                    Text = r.Name!
                })
                .ToListAsync(cancellationToken);
        }

        public async Task LoadRolesAsync(ChangeRoleVM vm)
        {
            var roles = await _roleManager.Roles
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem
                {
                    Value = r.Name!,
                    Text = r.Name!
                })
                .ToListAsync();

            vm.Roles = roles;
        }

        public async Task<ServiceResult> LockAsync(string id,CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return ServiceResult.Failure("User not found.");
            var currentRole = await _context.UserRoles.Where(ur => ur.UserId == user.Id)
                .Join(_context.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name)
                .FirstOrDefaultAsync(cancellationToken);

            if (currentRole == SD.ADMIN_ROLE || currentRole == SD.SUPER_ADMIN_ROLE)
            {
                return ServiceResult.Failure("Admin accounts cannot be locked.");
            }
            if (user.LockoutEnd.HasValue &&
                user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                return ServiceResult.Failure("User is already locked.");
            }


            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return ServiceResult.Failure(
                    string.Join(Environment.NewLine,
                        result.Errors.Select(e => e.Description)));
            }

            return ServiceResult.SuccessResult("User locked successfully.");
        }

        public async Task<ServiceResult> UnlockAsync(string id,CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return ServiceResult.Failure("User not found.");

            if (!user.LockoutEnd.HasValue ||
                user.LockoutEnd <= DateTimeOffset.UtcNow)
            {
                return ServiceResult.Failure("User is already unlocked.");
            }

            user.LockoutEnd = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return ServiceResult.Failure(
                    string.Join(Environment.NewLine,
                        result.Errors.Select(e => e.Description)));
            }

            return ServiceResult.SuccessResult("User unlocked successfully.");
        }
    }
}
