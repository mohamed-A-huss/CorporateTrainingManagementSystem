namespace CorporateTrainingManagementSystem.ViewModels.UserManagement
{
    public class UserDetailsVM
    {
        public string Id { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int Points { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool IsLocked { get; set; }
    }
}
