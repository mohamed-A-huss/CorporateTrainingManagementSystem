namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorExamsVM
    {
        public int CourseId { get; set; }

        public IEnumerable<InstructorExamVM> Exams { get; set; }
            = Enumerable.Empty<InstructorExamVM>();
    }
}
