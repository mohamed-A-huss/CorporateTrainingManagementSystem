namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Exam
{
    public class ExamResultVM
    {
        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public int ExamId { get; set; }

        public double Score { get; set; }

        public double TotalMarks { get; set; }

        public double PassMark { get; set; }

        public bool Passed { get; set; }

        public int AttemptsUsed { get; set; }

        public int AttemptsRemaining { get; set; }

        public bool HasCertificate { get; set; }
    }
}
