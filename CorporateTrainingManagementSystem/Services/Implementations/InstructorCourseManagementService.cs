namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class InstructorCourseManagementService : IInstructorCourseManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public InstructorCourseManagementService(
            IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }
        // Lesson Management Methods
        public async Task<ServiceResult> CreateLessonAsync(CreateLessonVM vm,string instructorId,CancellationToken cancellationToken = default)
        {
            // Check Course Ownership

            var course = (await _unitOfWork.Courses.GetAsync(
                c => c.CourseId == vm.CourseId &&
                     c.InstructorId == instructorId,
                cancellationToken: cancellationToken))
                .FirstOrDefault();

            if (course is null)
            {
                return ServiceResult.Failure("Course not found.");
            }

            string? pdfPath = null;

            // Upload PDF

            if (vm.PdfFile is not null)
            {
                var uploadResult =
                    await _fileService.UploadFileAsync(
                        vm.PdfFile,
                        UploadFolders.Pdfs,
                        cancellationToken);

                if (!uploadResult.Success)
                {
                    return ServiceResult.Failure(
                        uploadResult.ErrorMessage!);
                }

                pdfPath = uploadResult.FilePath;
            }

            // Create Lesson

            var lesson = new Lesson
            {
                Title = vm.Title,

                Content = vm.Content,

                VideoUrl = vm.VideoUrl,

                PdfPath = pdfPath,

                CourseId = vm.CourseId
            };

            await _unitOfWork.Lessons.CreateAsync(
                lesson,
                cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult("Lesson created successfully.");
        }


        public async Task<InstructorEditLessonVM?> GetLessonForEditAsync(int lessonId,string instructorId,CancellationToken cancellationToken = default)
        {
            var lesson = await _unitOfWork.Lessons.GetOneAsync(
                l => l.LessonId == lessonId,
                includes:
                [
                    l => l.Course
                ],
                cancellationToken: cancellationToken);

            if (lesson is null)
                return null;

            if (lesson.Course.InstructorId != instructorId)
                return null;

            return new InstructorEditLessonVM
            {
                LessonId = lesson.LessonId,

                CourseId = lesson.CourseId,

                Title = lesson.Title,

                Content = lesson.Content,

                VideoUrl = lesson.VideoUrl,

                ExistingPdf = lesson.PdfPath
            };
        }

        public async Task<ServiceResult> UpdateLessonAsync(InstructorEditLessonVM vm,string instructorId,CancellationToken cancellationToken = default)
        {
            var lesson = await _unitOfWork.Lessons.GetOneAsync(
                l => l.LessonId == vm.LessonId,
                includes:
                [
                    l => l.Course
                ],
                cancellationToken: cancellationToken);

            if (lesson is null)
                return ServiceResult.Failure("Lesson not found.");

            if (lesson.Course.InstructorId != instructorId)
                return ServiceResult.Failure("Unauthorized.");

            lesson.Title = vm.Title;

            lesson.Content = vm.Content;

            lesson.VideoUrl = vm.VideoUrl;

            if (vm.PdfFile is not null)
            {
                _fileService.DeleteFile(vm.ExistingPdf);

                var uploadResult =
                    await _fileService.UploadFileAsync(
                        vm.PdfFile,
                        UploadFolders.Pdfs,
                        cancellationToken);

                if (!uploadResult.Success)
                    return ServiceResult.Failure(uploadResult.ErrorMessage!);

                lesson.PdfPath = uploadResult.FilePath;
            }

            _unitOfWork.Lessons.Update(lesson);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult("Lesson updated successfully.");
        }
        public async Task<ServiceResult> DeleteLessonAsync(int lessonId,string instructorId,CancellationToken cancellationToken = default)
        {
            var lesson = await _unitOfWork.Lessons.GetOneAsync(
                l => l.LessonId == lessonId,
                includes:
                [
                    l => l.Course
                ],
                cancellationToken: cancellationToken);

            if (lesson is null)
                return ServiceResult.Failure("Lesson not found.");

            if (lesson.Course.InstructorId != instructorId)
                return ServiceResult.Failure("Unauthorized.");

            if (!string.IsNullOrWhiteSpace(lesson.PdfPath))
            {
                _fileService.DeleteFile(lesson.PdfPath);
            }

            _unitOfWork.Lessons.Delete(lesson);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult("Lesson deleted successfully.");
        }
        //Exam Management Methods
        public async Task<ServiceResult> CreateExamAsync(InstructorCreateExamVM vm,string instructorId,CancellationToken cancellationToken = default)
        {
            var course = await _unitOfWork.Courses.GetOneAsync(
                c => c.CourseId == vm.CourseId,
                cancellationToken: cancellationToken);

            if (course is null)
                return ServiceResult.Failure("Course not found.");

            if (course.InstructorId != instructorId)
                return ServiceResult.Failure("Unauthorized.");

            var exam = new Exam
            {
                Title = vm.Title,
                DurationMinutes = vm.DurationMinutes,
                PassMark = vm.PassMark,
                TotalMarks = vm.TotalMarks,
                CourseId = vm.CourseId
            };

            await _unitOfWork.Exams.CreateAsync(exam, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult("Exam created successfully.");
        }
        public async Task<ServiceResult> UpdateExamAsync(InstructorEditExamVM vm,string instructorId,CancellationToken cancellationToken = default)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == vm.ExamId,
                includes:
                [
                    e => e.Course
                ],
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult.Failure("Exam not found.");

            if (exam.Course.InstructorId != instructorId)
                return ServiceResult.Failure("Unauthorized.");

            exam.Title = vm.Title;
            exam.DurationMinutes = vm.DurationMinutes;
            exam.PassMark = vm.PassMark;
            exam.TotalMarks = vm.TotalMarks;

            _unitOfWork.Exams.Update(exam);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult("Exam updated successfully.");
        }
        public async Task<ServiceResult> DeleteExamAsync(int examId,string instructorId,CancellationToken cancellationToken = default)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == examId,
                includes:
                [
                    e => e.Course
                ],
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult.Failure("Exam not found.");

            if (exam.Course.InstructorId != instructorId)
                return ServiceResult.Failure("Unauthorized.");

            _unitOfWork.Exams.Delete(exam);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult("Exam deleted successfully.");
        }
    }
}
