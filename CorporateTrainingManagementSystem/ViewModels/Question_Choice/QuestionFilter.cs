namespace CorporateTrainingManagementSystem.ViewModels.Question
{
    public class QuestionFilter
    {
        public int? CourseId { get; set; }

        public int? ExamId { get; set; }

        public QuestionType? QuestionType { get; set; }

        public string? QuestionText { get; set; }

        public IEnumerable<SelectListItem> Courses { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Exams { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
