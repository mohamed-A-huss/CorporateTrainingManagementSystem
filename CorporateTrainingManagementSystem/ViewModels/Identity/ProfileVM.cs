namespace CorporateTrainingManagementSystem.ViewModels.Identity
{
    public class ProfileVM
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? ProfilePicture { get; set; }

        public string Department { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool EmailConfirmed { get; set; }
    }
}
