namespace CorporateTrainingManagementSystem.ViewModels.Question
{
    public class QuestionVM
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public QuestionType QuestionType { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public string ExamTitle { get; set; } = string.Empty;

        public int ChoicesCount { get; set; }
        public List<ChoiceVM> Choices { get; set; } = [];
    }
}
