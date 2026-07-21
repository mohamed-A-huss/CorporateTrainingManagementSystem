namespace CorporateTrainingManagementSystem.Models
{
    public class Badge
    {
        public int BadgeId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int RequiredPoints { get; set; }
        public string Icon { get; set; } = "fa-award";
        public string Color { get; set; } = "text-warning";

        public ICollection<UserBadge> UserBadges { get; set; }
    }
}
