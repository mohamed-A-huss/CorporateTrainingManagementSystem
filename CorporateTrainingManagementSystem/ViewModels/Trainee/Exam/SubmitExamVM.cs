namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Exam
{
    public class SubmitExamVM
    {
        public int AttemptId { get; set; }

        public List<QuestionAnswerVM> Answers { get; set; } = [];
    }
}
