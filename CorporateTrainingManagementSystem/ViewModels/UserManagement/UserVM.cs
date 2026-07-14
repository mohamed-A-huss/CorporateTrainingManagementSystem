namespace CorporateTrainingManagementSystem.ViewModels.UserManagement
{
    public class UserVM
    {
        public string Id { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public string Department { get; set; } = string.Empty;

        public int Points { get; set; }

        public bool IsLocked { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
