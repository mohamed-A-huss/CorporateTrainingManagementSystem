namespace CorporateTrainingManagementSystem.ViewModels.Exam
{
    public class PaginatedExam
    {
        public IEnumerable<ExamVM> Exams { get; set; }
            = Enumerable.Empty<ExamVM>();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public ExamFilter? Filter { get; set; }
    }
}
