namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Badge
{
    public class TraineeBadgeVM
    {
        public int BadgeId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string? Description { get; set; }

        public DateTime AwardedDate { get; set; }
    }
}
