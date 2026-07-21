namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Exam
{
    public class TraineeTakeExamVM
    {
        public int AttemptId { get; set; }

        public int ExamId { get; set; }

        public string ExamTitle { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public DateTime EndTime { get; set; }

        public List<TraineeQuestionVM> Questions { get; set; } = [];
    }
}
