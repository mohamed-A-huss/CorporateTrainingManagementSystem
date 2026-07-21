using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CorporateTrainingManagementSystem.ViewModels.Exam
{
    public class EditExamVM
    {
        public int ExamId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Duration (Minutes)")]
        [Range(1, 300)]
        public int DurationMinutes { get; set; }

        [Display(Name = "Pass Mark")]
        [Range(0, 1000)]
        public int PassMark { get; set; }
        [Range(1, 10)]
        public int MaxAttempts { get; set; }

        [Display(Name = "Total Marks")]
        [Range(1, 1000)]
        public int TotalMarks { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        public IEnumerable<SelectListItem> Courses { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
