namespace CorporateTrainingManagementSystem.ViewModels.Question
{
    public class PaginatedQuestion
    {
        public IEnumerable<QuestionVM> Questions { get; set; }
            = Enumerable.Empty<QuestionVM>();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public QuestionFilter? Filter { get; set; }
    }
}
