namespace CorporateTrainingManagementSystem.Models
{
    public class LessonProgress
    {
        public int LessonProgressId { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int LessonId { get; set; }

        public Lesson Lesson { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedDate { get; set; }
    }
}
