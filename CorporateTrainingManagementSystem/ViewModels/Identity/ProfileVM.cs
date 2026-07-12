namespace CorporateTrainingManagementSystem.ViewModels.Identity
{
    public class ProfileVM
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? ProfilePicture { get; set; }
    }
}
