namespace CorporateTrainingManagementSystem.Models
{
    public class UserBadge
    {
        public int UserBadgeId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int BadgeId { get; set; }

        public Badge Badge { get; set; }

        public DateTime AwardedDate { get; set; } = DateTime.Now;
    }
}
