namespace CorporateTrainingManagementSystem.ViewModels.Badge
{
    public class PaginatedBadge
    {
        public IEnumerable<BadgeVM> Badges { get; set; } = [];

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
        public string? Query { get; set; }
        public string? Description { get; set; }
        public int? MaxRequiredPoints { get; set; }
        public int? MinRequiredPoints { get; set; }

    }
}
