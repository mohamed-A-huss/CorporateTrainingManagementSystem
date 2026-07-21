namespace CorporateTrainingManagementSystem.Models
{
    public class ExamAnswer
    {
        public int ExamAnswerId { get; set; }

        public int AttemptId { get; set; }

        public ExamAttempt Attempt { get; set; } = null!;

        public int QuestionId { get; set; }

        public Question Question { get; set; } = null!;

        public int ChoiceId { get; set; }

        public Choice Choice { get; set; } = null!;
    }
}
