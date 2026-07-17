namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorLessonsVM
    {
        public int CourseId { get; set; }

        public IEnumerable<LessonVM> Lessons { get; set; }
    }
}
