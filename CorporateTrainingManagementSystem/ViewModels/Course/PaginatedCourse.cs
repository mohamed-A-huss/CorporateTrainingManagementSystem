namespace CorporateTrainingManagementSystem.ViewModels.Course
{
    public class PaginatedCourse
    {
        public IEnumerable<CourseVM> Courses { get; set; }
            = Enumerable.Empty<CourseVM>();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public CourseFilter? Filter { get; set; }
    }
}
