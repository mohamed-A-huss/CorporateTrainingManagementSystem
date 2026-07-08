namespace CorporateTrainingManagementSystem.ViewModels.Lesson
{
    public class PaginatedLesson
    {
        public IEnumerable<LessonVM> Lessons { get; set; }
            = Enumerable.Empty<LessonVM>();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public LessonFilter? Filter { get; set; }
    }
}
