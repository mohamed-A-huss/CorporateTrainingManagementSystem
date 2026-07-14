using Microsoft.AspNetCore.Identity;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public EnrollmentService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<ServiceResult> CreateAsync(CreateEnrollmentVM vm,CancellationToken cancellationToken = default)
        {
            // Check User

            var user = await _userManager.FindByIdAsync(vm.TraineeId);

            if (user is null)
                return ServiceResult.Failure("Selected trainee was not found.");

            // Ensure user is really a Trainee

            if (!await _userManager.IsInRoleAsync(user, SD.TRAINEE_ROLE))
                return ServiceResult.Failure("Selected user is not a trainee.");

            // Check Course

            var course = await _unitOfWork.Courses.GetOneAsync(
                c => c.CourseId == vm.CourseId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (course is null)
                return ServiceResult.Failure("Selected course was not found.");

            // Prevent duplicate enrollment

            var existingEnrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.TraineeId == vm.TraineeId &&
                     e.CourseId == vm.CourseId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (existingEnrollment is not null)
                return ServiceResult.Failure("This trainee is already enrolled in this course.");

            var enrollment = new Enrollment
            {
                TraineeId = vm.TraineeId,
                CourseId = vm.CourseId,
                EnrollmentDate = DateTime.UtcNow,
                Status = EnrollmentStatus.Active
            };

            await _unitOfWork.Enrollments.CreateAsync(
                enrollment,
                cancellationToken);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to create enrollment.");

            return ServiceResult.SuccessResult("Enrollment created successfully.");
        }
        

        public async Task<PaginatedEnrollment> GetAllAsync(int page = 1,int pageSize = 10,EnrollmentFilter? filter = null,CancellationToken cancellationToken = default)
        {
            filter ??= new EnrollmentFilter();

            var enrollments = await _unitOfWork.Enrollments.GetAsync(
                includes: [e => e.User, e => e.Course],
                tracked: false,
                cancellationToken: cancellationToken);

            if (!string.IsNullOrWhiteSpace(filter.TraineeId))
            {
                enrollments = enrollments.Where(e =>
                    e.TraineeId == filter.TraineeId);
            }

            if (filter.CourseId.HasValue)
            {
                enrollments = enrollments.Where(e =>
                    e.CourseId == filter.CourseId.Value);
            }

            if (filter.Status.HasValue)
            {
                enrollments = enrollments.Where(e =>
                    e.Status == filter.Status.Value);
            }

            filter.Trainees = (await _userManager.GetUsersInRoleAsync(SD.TRAINEE_ROLE))
                .OrderBy(u => u.FullName)
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.FullName
                });

            var courses = await _unitOfWork.Courses.GetAsync(
                tracked: false,
                cancellationToken: cancellationToken);

            filter.Courses = courses
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.Title
                });

            var totalCount = enrollments.Count();

            var items = enrollments
                .OrderByDescending(e => e.EnrollmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EnrollmentVM
                {
                    EnrollmentId = e.EnrollmentId,

                    TraineeId = e.TraineeId,

                    TraineeName = e.User.FullName,

                    CourseId = e.CourseId,

                    CourseTitle = e.Course.Title,

                    EnrollmentDate = e.EnrollmentDate,

                    Status = e.Status
                });

            return new PaginatedEnrollment
            {
                Enrollments = items,

                CurrentPage = page,

                TotalCount = totalCount,

                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),

                Filter = filter
            };
        }

        public async Task<EnrollmentVM?> GetByIdAsync(int id,CancellationToken cancellationToken = default)
        {
            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.EnrollmentId == id,
                includes: [e => e.User, e => e.Course],
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment is null)
                return null;

            return new EnrollmentVM
            {
                EnrollmentId = enrollment.EnrollmentId,

                TraineeId = enrollment.TraineeId,

                TraineeName = enrollment.User.FullName,

                CourseId = enrollment.CourseId,

                CourseTitle = enrollment.Course.Title,

                EnrollmentDate = enrollment.EnrollmentDate,

                Status = enrollment.Status
            };
        }

        public async Task<ServiceResult> DeleteAsync(int id,CancellationToken cancellationToken = default)
        {
            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.EnrollmentId == id,
                tracked: true,
                cancellationToken: cancellationToken);

            if (enrollment is null)
                return ServiceResult.Failure("Enrollment was not found.");

            _unitOfWork.Enrollments.Delete(enrollment);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to delete enrollment.");

            return ServiceResult.SuccessResult("Enrollment deleted successfully.");
        }
        public async Task LoadDropdownsAsync(CreateEnrollmentVM vm,CancellationToken cancellationToken = default)
        {
            var trainees = await _userManager.GetUsersInRoleAsync(SD.TRAINEE_ROLE);

            vm.Trainees = trainees
                .OrderBy(t => t.FullName)
                .Select(t => new SelectListItem
                {
                    Value = t.Id,
                    Text = t.FullName
                });

            var courses = await _unitOfWork.Courses.GetAsync(
                tracked: false,
                cancellationToken: cancellationToken);

            vm.Courses = courses
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.Title
                });
        }

        public async Task<ServiceResult> ChangeStatusAsync(int enrollmentId,EnrollmentStatus status,CancellationToken cancellationToken = default)
        {
            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.EnrollmentId == enrollmentId,
                tracked: true,
                cancellationToken: cancellationToken);

            if (enrollment is null)
                return ServiceResult.Failure("Enrollment was not found.");

            // Same Status

            if (enrollment.Status == status)
                return ServiceResult.Failure("Enrollment is already in the selected status.");

            // Business Rules

            switch (enrollment.Status)
            {
                case EnrollmentStatus.Active:

                    // Active -> Completed OR Cancelled
                    break;

                case EnrollmentStatus.Completed:

                    return ServiceResult.Failure(
                        "Completed enrollments cannot be modified.");

                case EnrollmentStatus.Cancelled:

                    return ServiceResult.Failure(
                        "Cancelled enrollments cannot be modified.");

                default:

                    return ServiceResult.Failure(
                        "Invalid enrollment status.");
            }

            enrollment.Status = status;

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to update enrollment status.");

            return ServiceResult.SuccessResult("Enrollment status updated successfully.");
        }
    }
}
