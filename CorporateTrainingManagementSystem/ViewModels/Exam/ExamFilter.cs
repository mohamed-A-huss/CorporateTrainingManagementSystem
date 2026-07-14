namespace CorporateTrainingManagementSystem.ViewModels.Exam
{
    public class ExamFilter
    {
        public string? Title { get; set; }

        public int? CourseId { get; set; }

        public int? MinPassMark { get; set; }

        public int? MaxPassMark { get; set; }
    }
}
