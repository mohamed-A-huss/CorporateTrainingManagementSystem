namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorQuestionVM
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public QuestionType QuestionType { get; set; }

        public int Mark { get; set; }

        public int ChoicesCount { get; set; }
    }
}
