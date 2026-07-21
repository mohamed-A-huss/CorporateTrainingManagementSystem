using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorEditExamVM
    {
        public int ExamId { get; set; }

        public int CourseId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Range(1, 300)]
        public int DurationMinutes { get; set; }

        [Range(1, 100)]
        public int PassMark { get; set; }
        [Range(1, 10)]
        public int MaxAttempts { get; set; }

        [Range(1, 500)]
        public int TotalMarks { get; set; }
    }
}
