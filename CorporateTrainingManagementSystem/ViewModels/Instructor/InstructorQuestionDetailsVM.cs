namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorQuestionDetailsVM
    {
        public int QuestionId { get; set; }

        public int ExamId { get; set; }

        public string ExamTitle { get; set; } = string.Empty;

        public string QuestionText { get; set; } = string.Empty;

        public QuestionType QuestionType { get; set; }

        public int Mark { get; set; }

        public List<InstructorChoiceVM> Choices { get; set; }
            = new();
    }
}
