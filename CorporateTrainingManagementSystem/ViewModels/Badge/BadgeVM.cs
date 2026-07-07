namespace CorporateTrainingManagementSystem.ViewModels.Badge
{
    public class BadgeVM
    {
        public int BadgeId { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int RequiredPoints { get; set; }
    }
}

