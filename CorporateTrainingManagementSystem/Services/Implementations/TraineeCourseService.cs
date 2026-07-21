
using Microsoft.AspNetCore.Identity;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class TraineeCourseService : ITraineeCourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public TraineeCourseService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<TraineeCourseDetailsVM?> GetCourseDetailsAsync(int courseId,string traineeId,CancellationToken cancellationToken = default)
        {
            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.CourseId == courseId &&
                     e.TraineeId == traineeId,
                includes:
                [
                    e => e.Course,
                    e => e.Course.Lessons,
                    e => e.Course.Exams
                ],
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment is null)
                return null;

            var lessonProgress = await _unitOfWork.LessonProgresses.GetAsync(
                lp => lp.UserId == traineeId,
                tracked: false,
                cancellationToken: cancellationToken);

            var lessons = enrollment.Course.Lessons
                .OrderBy(l => l.LessonId)
                .Select(l => new TraineeLessonVM
                {
                    LessonId = l.LessonId,
                    Title = l.Title,
                    IsCompleted = lessonProgress.Any(lp =>
                        lp.LessonId == l.LessonId &&
                        lp.IsCompleted)
                })
                .ToList();

            var completedLessons = lessons.Count(l => l.IsCompleted);

            var totalLessons = lessons.Count;

            var progress = totalLessons == 0 ? 0 : Math.Round((double)completedLessons / totalLessons * 100, 0);

            var exam = enrollment.Course.Exams.FirstOrDefault();
            var attempts = exam is null
                ? Enumerable.Empty<ExamAttempt>()
                : await _unitOfWork.ExamAttempts.GetAsync(
                    ea => ea.ExamId == exam.ExamId &&
                          ea.UserId == traineeId,
                    tracked: false,
                    cancellationToken: cancellationToken);

            var attemptsUsed = attempts.Count();

            var hasPassed = attempts.Any(a => a.IsPassed);

            return new TraineeCourseDetailsVM
            {
                CourseId = enrollment.CourseId,

                Title = enrollment.Course.Title,

                Description = enrollment.Course.Description,

                TotalLessons = totalLessons,

                CompletedLessons = completedLessons,

                ProgressPercentage = progress,

                Lessons = lessons,

                ExamId = exam.ExamId ,

                MaxAttempts = exam?.MaxAttempts ?? 0,

                AttemptsUsed = attemptsUsed,

                HasRemainingAttempts = exam is not null &&
                           attemptsUsed < exam.MaxAttempts,

                CanTakeExam = exam is not null &&
                              totalLessons > 0 &&
                              completedLessons == totalLessons &&
                              !hasPassed &&
                              attemptsUsed < exam.MaxAttempts
            };
        }

        public async Task<IEnumerable<TraineeMyCourseVM>> GetMyCoursesAsync(string traineeId,CancellationToken cancellationToken = default)
        {
            var enrollments = await _unitOfWork.Enrollments.GetAsync(
                e => e.TraineeId == traineeId,
                includes:
                [
                    e => e.Course,
                    e => e.Course.Lessons
                ],
                tracked: false,
                cancellationToken: cancellationToken);

            var lessonProgress = await _unitOfWork.LessonProgresses.GetAsync(
                lp => lp.UserId == traineeId && lp.IsCompleted,
                tracked: false,
                cancellationToken: cancellationToken);

            return enrollments.Select(enrollment =>
            {
                var lessonsCount = enrollment.Course.Lessons.Count;

                var completedLessons = enrollment.Course.Lessons.Count(lesson =>
                    lessonProgress.Any(lp => lp.LessonId == lesson.LessonId));

                var progress = lessonsCount == 0
                    ? 0
                    : Math.Round((double)completedLessons / lessonsCount * 100, 0);

                return new TraineeMyCourseVM
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    CourseId = enrollment.CourseId,

                    Title = enrollment.Course.Title,
                    Description = enrollment.Course.Description,

                    EnrollmentDate = enrollment.EnrollmentDate,
                    Status = enrollment.Status,

                    LessonsCount = lessonsCount,
                    CompletedLessons = completedLessons,
                    ProgressPercentage = progress
                };
            })
            .OrderBy(c => c.Status)
            .ThenBy(c => c.Title)
            .ToList();
        }
        public async Task<TraineeLessonDetailsVM?> GetLessonDetailsAsync(int lessonId,string traineeId,CancellationToken cancellationToken = default)
        {
            var lesson = await _unitOfWork.Lessons.GetOneAsync(
                l => l.LessonId == lessonId,
                includes:
                [
                    l => l.Course
                ],
                tracked: false,
                cancellationToken: cancellationToken);

            if (lesson is null)
                return null;

            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.CourseId == lesson.CourseId &&
                     e.TraineeId == traineeId &&
                     e.Status == EnrollmentStatus.Active,
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment is null)
                return null;

            var progress = await _unitOfWork.LessonProgresses.GetOneAsync(
                lp => lp.UserId == traineeId &&
                      lp.LessonId == lessonId,
                tracked: false,
                cancellationToken: cancellationToken);

            var lessons = (await _unitOfWork.Lessons.GetAsync(
                l => l.CourseId == lesson.CourseId,
                tracked: false,
                cancellationToken: cancellationToken))
                .OrderBy(l => l.Order)
                .ToList();

            var currentIndex = lessons.FindIndex(l => l.LessonId == lessonId);

            var vm = new TraineeLessonDetailsVM
            {
                LessonId = lesson.LessonId,
                CourseId = lesson.CourseId,

                Title = lesson.Title,
                Content = lesson.Content,
                VideoUrl = lesson.VideoUrl,
                PdfPath = lesson.PdfPath,

                IsCompleted = progress?.IsCompleted ?? false,

                HasPreviousLesson = currentIndex > 0,
                HasNextLesson = currentIndex < lessons.Count - 1
            };

            if (vm.HasPreviousLesson)
            {
                vm.PreviousLessonId = lessons[currentIndex - 1].LessonId;
            }

            if (vm.HasNextLesson)
            {
                vm.NextLessonId = lessons[currentIndex + 1].LessonId;
            }

            return vm;
        }

        public async Task<ServiceResult> MarkLessonAsReadAsync(int lessonId,string traineeId,CancellationToken cancellationToken = default)
        {
            var lesson = await _unitOfWork.Lessons.GetOneAsync(
                l => l.LessonId == lessonId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (lesson is null)
                return ServiceResult.Failure("Lesson not found.");

            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.CourseId == lesson.CourseId &&
                     e.TraineeId == traineeId &&
                     e.Status == EnrollmentStatus.Active,
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment is null)
                return ServiceResult.Failure("You are not enrolled in this course.");

            var progress = await _unitOfWork.LessonProgresses.GetOneAsync(
                lp => lp.UserId == traineeId &&
                      lp.LessonId == lessonId,
                cancellationToken: cancellationToken);

            if (progress is not null)
            {
                if (!progress.IsCompleted)
                {
                    progress.IsCompleted = true;
                    progress.CompletedDate = DateTime.Now;

                    _unitOfWork.LessonProgresses.Update(progress);

                    await _unitOfWork.CommitAsync(cancellationToken);
                }

                return ServiceResult.SuccessResult("Lesson marked as completed.");
            }

            progress = new LessonProgress
            {
                UserId = traineeId,
                LessonId = lessonId,
                IsCompleted = true,
                CompletedDate = DateTime.Now
            };

            await _unitOfWork.LessonProgresses.CreateAsync(
                progress,
                cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult("Lesson marked as completed.");
        }

        public async Task<TraineeExamStartVM?> GetExamStartAsync(int examId,string traineeId,CancellationToken cancellationToken = default)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == examId,
                includes:
                [
                    e => e.Course,
            e => e.Questions
                ],
                tracked: false,
                cancellationToken: cancellationToken);

            if (exam is null)
                return null;

            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.CourseId == exam.CourseId &&
                     e.TraineeId == traineeId &&
                     e.Status == EnrollmentStatus.Active,
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment is null)
                return null;

            var lessons = await _unitOfWork.Lessons.GetAsync(
                l => l.CourseId == exam.CourseId,
                tracked: false,
                cancellationToken: cancellationToken);

            var lessonIds = lessons
                .Select(l => l.LessonId)
                .ToList();

            var completedLessons = await _unitOfWork.LessonProgresses.GetAsync(
                lp => lp.UserId == traineeId &&
                      lp.IsCompleted &&
                      lessonIds.Contains(lp.LessonId),
                tracked: false,
                cancellationToken: cancellationToken);

            bool completedCourse =
                lessonIds.Count > 0 &&
                completedLessons.Count() == lessonIds.Count;

            var attempts = await _unitOfWork.ExamAttempts.GetAsync(
                a => a.ExamId == examId &&
                     a.UserId == traineeId,
                tracked: false,
                cancellationToken: cancellationToken);

            int attemptsUsed = attempts.Count();

            bool hasRemainingAttempts =
                attemptsUsed < exam.MaxAttempts;

            return new TraineeExamStartVM
            {
                ExamId = exam.ExamId,
                CourseId = exam.CourseId,

                CourseTitle = exam.Course.Title,
                ExamTitle = exam.Title,

                DurationMinutes = exam.DurationMinutes,
                PassMark = exam.PassMark,
                TotalMarks = exam.TotalMarks,

                QuestionsCount = exam.Questions.Count,

                MaxAttempts = exam.MaxAttempts,
                AttemptsUsed = attemptsUsed,

                CanStart = completedCourse &&
                           hasRemainingAttempts
            };
        }
        public async Task<ServiceResult<int>> BeginExamAsync(int examId,string traineeId,CancellationToken cancellationToken = default)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == examId,
                includes:
                [
                    e => e.Course,
                    e => e.Questions
                ],
                tracked: false,
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult<int>.Failure("Exam not found.");

            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.CourseId == exam.CourseId &&
                     e.TraineeId == traineeId &&
                     e.Status == EnrollmentStatus.Active,
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment is null)
                return ServiceResult<int>.Failure("You are not enrolled in this course.");

            var attempts = await _unitOfWork.ExamAttempts.GetAsync(
                a => a.ExamId == examId &&
                     a.UserId == traineeId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (attempts.Count() >= exam.MaxAttempts)
                return ServiceResult<int>.Failure("You have reached the maximum number of attempts.");

            var attempt = new ExamAttempt
            {
                UserId = traineeId,
                ExamId = examId,
                StartTime = DateTime.Now,
                Score = 0,
                IsPassed = false
            };

            await _unitOfWork.ExamAttempts.CreateAsync(
                attempt,
                cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult<int>.SuccessResult(
                attempt.AttemptId,
                "Exam started successfully.");
        }
        public async Task<TraineeTakeExamVM?> GetTakeExamAsync(int attemptId,string traineeId,CancellationToken cancellationToken = default)
        {
            var attempt = await _unitOfWork.ExamAttempts.GetOneAsync(
                a => a.AttemptId == attemptId &&
                     a.UserId == traineeId,
                includes:
                [
                    a => a.Exam
                ],
                tracked: false,
                cancellationToken: cancellationToken);

            if (attempt is null)
                return null;

            if (attempt.EndTime is not null)
                return null;

            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == attempt.ExamId,
                includes:
                [
                    e => e.Questions
                ],
                tracked: false,
                cancellationToken: cancellationToken);

            if (exam is null)
                return null;

            var questions = await _unitOfWork.Questions.GetAsync(
                q => q.ExamId == exam.ExamId,
                includes:
                [
                    q => q.Choices
                ],
                tracked: false,
                cancellationToken: cancellationToken);

            return new TraineeTakeExamVM
            {
                AttemptId = attempt.AttemptId,

                ExamId = exam.ExamId,

                ExamTitle = exam.Title,

                DurationMinutes = exam.DurationMinutes,

                EndTime = attempt.StartTime.AddMinutes(exam.DurationMinutes),

                Questions = questions
                    .OrderBy(q => q.QuestionId)
                    .Select(q => new TraineeQuestionVM
                    {
                        QuestionId = q.QuestionId,

                        QuestionText = q.QuestionText,

                        QuestionType = q.QuestionType,

                        Choices = q.Choices
                            .OrderBy(c => c.ChoiceId)
                            .Select(c => new TraineeChoiceVM
                            {
                                ChoiceId = c.ChoiceId,

                                ChoiceText = c.ChoiceText
                            })
                            .ToList()

                    }).ToList()
            };
        }
        public async Task<ServiceResult<ExamResultVM>> FinishExamAsync(int attemptId,string traineeId,Dictionary<int, int?> answers,CancellationToken cancellationToken = default)
        {
            var attempt = await _unitOfWork.ExamAttempts.GetOneAsync(
                a => a.AttemptId == attemptId &&
                     a.UserId == traineeId,
                includes:
                [
                    a => a.Exam,
            a => a.Exam.Course
                ],
                cancellationToken: cancellationToken);

            if (attempt is null)
                return ServiceResult<ExamResultVM>.Failure("Exam attempt not found.");

            if (attempt.EndTime is not null)
                return ServiceResult<ExamResultVM>.Failure("This exam has already been submitted.");

            var questions = await _unitOfWork.Questions.GetAsync(
                q => q.ExamId == attempt.ExamId,
                includes:
                [
                    q => q.Choices
                ],
                cancellationToken: cancellationToken);

            double score = 0;

            foreach (var question in questions)
            {
                answers.TryGetValue(question.QuestionId, out var selectedChoiceId);

                if (selectedChoiceId.HasValue)
                {
                    await _unitOfWork.ExamAnswers.CreateAsync(
                        new ExamAnswer
                        {
                            AttemptId = attempt.AttemptId,
                            QuestionId = question.QuestionId,
                            ChoiceId = selectedChoiceId.Value
                        },
                        cancellationToken);
                }

                var correctChoice = question.Choices
                    .FirstOrDefault(c => c.IsCorrect);

                if (correctChoice is not null &&
                    selectedChoiceId.HasValue &&
                    correctChoice.ChoiceId == selectedChoiceId.Value)
                {
                    score += question.Mark;
                }
            }

            attempt.Score = score;
            attempt.IsPassed = score >= attempt.Exam.PassMark;
            attempt.EndTime = DateTime.Now;

            _unitOfWork.ExamAttempts.Update(attempt);

            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.TraineeId == traineeId &&
                     e.CourseId == attempt.Exam.CourseId,
                cancellationToken: cancellationToken);

            if (attempt.IsPassed &&
                enrollment is not null &&
                enrollment.Status != EnrollmentStatus.Completed)
            {
                enrollment.Status = EnrollmentStatus.Completed;

                _unitOfWork.Enrollments.Update(enrollment);

                var certificate = new Certificate
                {
                    UserId = traineeId,
                    CourseId = attempt.Exam.CourseId,
                    IssueDate = DateTime.Now,
                    CertificateNumber = $"CERT-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}"
                };

                await _unitOfWork.Certificates.CreateAsync(
                    certificate,
                    cancellationToken);

                var user = await _userManager.FindByIdAsync(traineeId);

                if (user is not null)
                {
                    user.Points += attempt.Exam.Course.RewardPoints;

                    var identityResult = await _userManager.UpdateAsync(user);

                    if (!identityResult.Succeeded)
                        return ServiceResult<ExamResultVM>.Failure("Failed to update user points.");

                    var badges = await _unitOfWork.Badges.GetAsync(
                        tracked: false,
                        cancellationToken: cancellationToken);

                    foreach (var badge in badges)
                    {
                        if (user.Points < badge.RequiredPoints)
                            continue;

                        var alreadyAwarded =
                            await _unitOfWork.UserBadges.GetOneAsync(
                                ub => ub.UserId == traineeId &&
                                      ub.BadgeId == badge.BadgeId,
                                tracked: false,
                                cancellationToken: cancellationToken);

                        if (alreadyAwarded is not null)
                            continue;

                        await _unitOfWork.UserBadges.CreateAsync(
                            new UserBadge
                            {
                                UserId = traineeId,
                                BadgeId = badge.BadgeId,
                                AwardedDate = DateTime.Now
                            },
                            cancellationToken);
                    }
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            var attemptsUsed = (await _unitOfWork.ExamAttempts.GetAsync(
                a => a.UserId == traineeId &&
                     a.ExamId == attempt.ExamId,
                tracked: false,
                cancellationToken: cancellationToken))
                .Count();

            var attemptsRemaining = attempt.Exam.MaxAttempts - attemptsUsed;

            var resultVm = new ExamResultVM
            {
                CourseId = attempt.Exam.CourseId,
                CourseTitle = attempt.Exam.Course.Title,
                ExamId = attempt.ExamId,

                Score = score,
                TotalMarks = attempt.Exam.TotalMarks,
                PassMark = attempt.Exam.PassMark,

                Passed = attempt.IsPassed,

                AttemptsUsed = attemptsUsed,
                AttemptsRemaining = Math.Max(0, attemptsRemaining),

                HasCertificate = attempt.IsPassed
            };

            return ServiceResult<ExamResultVM>.SuccessResult(
                resultVm,
                attempt.IsPassed
                    ? "Congratulations! You passed the exam."
                    : "Unfortunately, you did not pass the exam.");
        }
        public async Task<ServiceResult<int>> StartExamAsync(int examId,string traineeId,CancellationToken cancellationToken = default)
        {
            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == examId,
                includes:
                [
                    e => e.Course
                ],
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult<int>.Failure("Exam not found.");

            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.TraineeId == traineeId &&
                     e.CourseId == exam.CourseId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment is null)
                return ServiceResult<int>.Failure("You are not enrolled in this course.");

            var attemptsUsed = (await _unitOfWork.ExamAttempts.GetAsync(
                a => a.UserId == traineeId &&
                     a.ExamId == examId,
                tracked: false,
                cancellationToken: cancellationToken))
                .Count();

            if (attemptsUsed >= exam.MaxAttempts)
                return ServiceResult<int>.Failure("You have used all available attempts.");

            var attempt = new ExamAttempt
            {
                UserId = traineeId,
                ExamId = examId,
                StartTime = DateTime.Now
            };

            await _unitOfWork.ExamAttempts.CreateAsync(
                attempt,
                cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult<int>.SuccessResult(
                attempt.AttemptId);
        }
        public async Task<List<BrowseCourseVM>> GetBrowseCoursesAsync(string traineeId,CancellationToken cancellationToken = default)
        {
            var courses = await _unitOfWork.Courses.GetAsync(

                includes:
                [
                    c => c.Instructor,
            c => c.Lessons
                ],

                tracked: false,

                cancellationToken: cancellationToken);

            var enrollments = await _unitOfWork.Enrollments.GetAsync(

                e => e.TraineeId == traineeId,

                tracked: false,

                cancellationToken: cancellationToken);

            var result = new List<BrowseCourseVM>();

            foreach (var course in courses)
            {
                var enrollment = enrollments.FirstOrDefault(
                    e => e.CourseId == course.CourseId);

                result.Add(new BrowseCourseVM
                {
                    CourseId = course.CourseId,

                    Title = course.Title,

                    Description = course.Description,

                    InstructorName = course.Instructor.FullName,

                    LessonsCount = course.Lessons.Count,

                    RewardPoints = course.RewardPoints,

                    IsEnrolled = enrollment?.Status == EnrollmentStatus.Active,

                    
                });
            }

            return result;
        }
        public async Task<ServiceResult> EnrollAsync(int courseId,string traineeId,CancellationToken cancellationToken = default)
        {
            var course = await _unitOfWork.Courses.GetOneAsync(
                c => c.CourseId == courseId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (course is null)
                return ServiceResult.Failure("Course not found.");

            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(
                e => e.CourseId == courseId &&
                     e.TraineeId == traineeId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (enrollment is not null)
                return ServiceResult.Failure("You are already enrolled in this course.");

            await _unitOfWork.Enrollments.CreateAsync(
                new Enrollment
                {
                    CourseId = courseId,

                    TraineeId = traineeId,

                    Status = EnrollmentStatus.Active,

                    EnrollmentDate = DateTime.Now
                },
                cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return ServiceResult.SuccessResult(
                "Successfully enrolled in the course.");
        }
        public async Task<PublicCourseDetailsVM?> GetPublicCourseDetailsAsync(int courseId,string traineeId,CancellationToken cancellationToken = default)
        {
            var course = await _unitOfWork.Courses.GetOneAsync(

                c => c.CourseId == courseId,

                includes:
                [
                    c => c.Instructor,
            c => c.Lessons
                ],

                tracked: false,

                cancellationToken: cancellationToken);

            if (course is null)
                return null;

            var enrollment = await _unitOfWork.Enrollments.GetOneAsync(

                e => e.CourseId == courseId &&
                     e.TraineeId == traineeId &&
                     e.Status == EnrollmentStatus.Active,

                tracked: false,

                cancellationToken: cancellationToken);

            return new PublicCourseDetailsVM
            {
                CourseId = course.CourseId,

                Title = course.Title,

                Description = course.Description,

                InstructorName = course.Instructor.FullName,

                LessonsCount = course.Lessons.Count,

                RewardPoints = course.RewardPoints,

                IsEnrolled = enrollment is not null
            };
        }
        public async Task<List<TraineeBadgeVM>> GetMyBadgesAsync(string traineeId,CancellationToken cancellationToken = default)
        {
            var badges = await _unitOfWork.UserBadges.GetAsync(

                ub => ub.UserId == traineeId,

                includes:
                [
                    ub => ub.Badge
                ],

                tracked: false,

                cancellationToken: cancellationToken);

            return badges
                .OrderByDescending(b => b.AwardedDate)
                .Select(b => new TraineeBadgeVM
                {
                    BadgeId = b.BadgeId,

                    Name = b.Badge.Name,
                    Icon = b.Badge.Icon,
                    Color = b.Badge.Color,
                    Description = b.Badge.Description,

                    AwardedDate = b.AwardedDate
                })
                .ToList();
        }
    }
    }

