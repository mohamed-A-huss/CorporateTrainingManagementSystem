using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorEditQuestionVM
    {
        public int QuestionId { get; set; }

        public int ExamId { get; set; }

        [Required]

        [MaxLength(500)]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public QuestionType QuestionType { get; set; }

        [Range(1, 100)]
        public int Mark { get; set; }
    }
}
