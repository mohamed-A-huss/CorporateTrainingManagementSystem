namespace CorporateTrainingManagementSystem.ViewModels.Course
{
    public class CourseFilter
    {
        public string? Title { get; set; }

        public string? InstructorId { get; set; }

        public DateTime? CreatedFrom { get; set; }

        public DateTime? CreatedTo { get; set; }
    }
}
