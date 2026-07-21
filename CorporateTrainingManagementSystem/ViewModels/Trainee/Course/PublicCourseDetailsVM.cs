namespace CorporateTrainingManagementSystem.ViewModels.Trainee.Course
{
    public class PublicCourseDetailsVM
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string InstructorName { get; set; } = string.Empty;

        public int LessonsCount { get; set; }

        public int RewardPoints { get; set; }

        public bool IsEnrolled { get; set; }
    }
}
