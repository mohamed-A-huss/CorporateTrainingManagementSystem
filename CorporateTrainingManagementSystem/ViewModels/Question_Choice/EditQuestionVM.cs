using CorporateTrainingManagementSystem.ViewModels.Question_Choice;
using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Question
{
    public class EditQuestionVM
    {
        public int QuestionId { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Exam")]
        public int ExamId { get; set; }

        [Required]
        [Display(Name = "Question")]
        [StringLength(500)]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Question Type")]
        public QuestionType QuestionType { get; set; }

        public List<EditChoiceVM> Choices { get; set; } = new()
                            {
                                new EditChoiceVM(),
                                new EditChoiceVM(),
                                new EditChoiceVM(),
                                new EditChoiceVM()
                            };
        [Required]
        [Display(Name = "Correct Answer")]
        [Range(1, 4)]
        public int CorrectChoice { get; set; }

        public IEnumerable<SelectListItem> Courses { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Exams { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
