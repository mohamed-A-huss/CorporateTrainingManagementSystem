namespace CorporateTrainingManagementSystem.Models
{
    public enum QuestionType
    {
        MCQ,
        TrueFalse
    }
    public class Question
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; }

        public QuestionType QuestionType { get; set; }
        public int Mark { get; set; }

        public int ExamId { get; set; }

        public Exam Exam { get; set; }

        public ICollection<Choice> Choices { get; set; }
    }
}
