using CorporateTrainingManagementSystem.ViewModels.Question_Choice;
using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Question
{
    public class CreateQuestionVM
    {
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
        [Required]
        [Display(Name = "Mark")]
        [Range(1, 100)]
        public int Mark { get; set; }


        public List<CreateChoiceVM> Choices { get; set; }= new()
                            {
                                new CreateChoiceVM(),
                                new CreateChoiceVM(),
                                new CreateChoiceVM(),
                                new CreateChoiceVM()
                            };
        [Required]
        [Display(Name = "Correct Answer")]
        public int CorrectChoice { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Exams { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
