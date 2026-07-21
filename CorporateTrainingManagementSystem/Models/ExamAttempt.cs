namespace CorporateTrainingManagementSystem.Models
{
    public class ExamAttempt
    {
        public int AttemptId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int ExamId { get; set; }

        public Exam Exam { get; set; }

        public double Score { get; set; }

        public bool IsPassed { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public ICollection<ExamAnswer> Answers { get; set; } = [];
    }
}
