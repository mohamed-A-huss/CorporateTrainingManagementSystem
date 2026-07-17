namespace CorporateTrainingManagementSystem.ViewModels.Instructor
{
    public class InstructorDashboardVM
    {
        public int CoursesCount { get; set; }

        public int StudentsCount { get; set; }

        public int LessonsCount { get; set; }

        public int ExamsCount { get; set; }

        public IEnumerable<InstructorCourseSummaryVM> AllCourses { get; set; }
    = Enumerable.Empty<InstructorCourseSummaryVM>();
    }
}
