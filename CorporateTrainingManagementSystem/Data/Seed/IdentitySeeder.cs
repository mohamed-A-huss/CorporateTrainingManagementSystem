using Microsoft.AspNetCore.Identity;

namespace CorporateTrainingManagementSystem.Data.Seed
{
    public static class IdentitySeeder
    {
        private const string AdminRole = "Admin";
        private const string InstructorRole = "Instructor";
        private const string StudentRole = "Student";
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);

            await SeedAdminAsync(userManager);

            await SeedInstructorsAsync(userManager);

            await SeedStudentsAsync(userManager);
        }
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[]{AdminRole,InstructorRole,StudentRole};

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }
        }
        private static async Task CreateUserIfNotExistsAsync(
    UserManager<ApplicationUser> userManager,
    string fullName,
    string userName,
    string email,
    string password,
    string role,
    int departmentId)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is not null)
                return;

            user = new ApplicationUser
            {
                FullName = fullName,
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
                Points = 0,
                DepartmentId = departmentId
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                throw new Exception(errors);
            }

            await userManager.AddToRoleAsync(user, role);
        }
        private static async Task SeedAdminAsync(
    UserManager<ApplicationUser> userManager)
        {
            await CreateUserIfNotExistsAsync(
                userManager,
                fullName: "System Administrator",
                userName: "admin",
                email: "admin@cts.com",
                password: "Admin@123",
                role: AdminRole,
                departmentId: 9); // IT
        }
        private static async Task SeedInstructorsAsync(
    UserManager<ApplicationUser> userManager)
        {
            await CreateUserIfNotExistsAsync(
                userManager,
                "Ahmed Hassan",
                "ahmed.hassan",
                "ahmed.hassan@cts.com",
                "Instructor@123",
                InstructorRole,
                9);

            await CreateUserIfNotExistsAsync(
                userManager,
                "Sara Ali",
                "sara.ali",
                "sara.ali@cts.com",
                "Instructor@123",
                InstructorRole,
                9);

            await CreateUserIfNotExistsAsync(
                userManager,
                "Mohamed Ibrahim",
                "mohamed.ibrahim",
                "mohamed.ibrahim@cts.com",
                "Instructor@123",
                InstructorRole,
                11); // R&D
        }
        private static async Task SeedStudentsAsync(
    UserManager<ApplicationUser> userManager)
        {
            await CreateUserIfNotExistsAsync(
                userManager,
                "Omar Adel",
                "omar.adel",
                "omar.adel@cts.com",
                "Student@123",
                StudentRole,
                5);

            await CreateUserIfNotExistsAsync(
                userManager,
                "Nour Mohamed",
                "nour.mohamed",
                "nour.mohamed@cts.com",
                "Student@123",
                StudentRole,
                6);

            await CreateUserIfNotExistsAsync(
                userManager,
                "Youssef Samy",
                "youssef.samy",
                "youssef.samy@cts.com",
                "Student@123",
                StudentRole,
                7);

            await CreateUserIfNotExistsAsync(
                userManager,
                "Salma Tarek",
                "salma.tarek",
                "salma.tarek@cts.com",
                "Student@123",
                StudentRole,
                10);

            await CreateUserIfNotExistsAsync(
                userManager,
                "Mahmoud Essam",
                "mahmoud.essam",
                "mahmoud.essam@cts.com",
                "Student@123",
                StudentRole,
                8);
        }
    }
}
