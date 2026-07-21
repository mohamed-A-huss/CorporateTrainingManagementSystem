namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Exam
{
    public class TraineeQuestionVM
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public QuestionType QuestionType { get; set; }

        public List<TraineeChoiceVM> Choices { get; set; } = [];
    }
}
