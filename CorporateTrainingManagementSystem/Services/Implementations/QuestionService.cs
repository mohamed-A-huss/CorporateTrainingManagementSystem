    
namespace CorporateTrainingManagementSystem.Services.Implementations
{
    
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public QuestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginatedQuestion> GetAllAsync(int page = 1,int pageSize = 10,QuestionFilter? filter = null,CancellationToken cancellationToken = default)
        {
            filter ??= new QuestionFilter();

            var questions = await _unitOfWork.Questions.GetAsync(
                includes: [q => q.Exam, q => q.Exam.Course, q => q.Choices],
                tracked: false,
                cancellationToken: cancellationToken);

            if (filter.CourseId.HasValue)
            {
                questions = questions.Where(q =>
                    q.Exam.CourseId == filter.CourseId.Value);
            }

            if (filter.ExamId.HasValue)
            {
                questions = questions.Where(q =>
                    q.ExamId == filter.ExamId.Value);
            }

            if (filter.QuestionType.HasValue)
            {
                questions = questions.Where(q =>
                    q.QuestionType == filter.QuestionType.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.QuestionText))
            {
                questions = questions.Where(q =>
                    q.QuestionText.Contains(
                        filter.QuestionText.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = questions.Count();

            var items = questions
                .OrderBy(q => q.QuestionId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new QuestionVM
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    CourseTitle = q.Exam.Course.Title,
                    ExamTitle = q.Exam.Title,
                    ChoicesCount = q.Choices.Count
                });

            // Load Courses
            filter.Courses = (await _unitOfWork.Courses.GetAsync(
                tracked: false,
                cancellationToken: cancellationToken))
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.Title
                });

            // Load Exams
            if (filter.CourseId.HasValue)
            {
                filter.Exams = (await _unitOfWork.Exams.GetAsync(
                    e => e.CourseId == filter.CourseId.Value,
                    tracked: false,
                    cancellationToken: cancellationToken))
                    .OrderBy(e => e.Title)
                    .Select(e => new SelectListItem
                    {
                        Value = e.ExamId.ToString(),
                        Text = e.Title
                    });
            }
            else
            {
                filter.Exams = Enumerable.Empty<SelectListItem>();
            }

            return new PaginatedQuestion
            {
                Questions = items,
                CurrentPage = page,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Filter = filter
            };
        }
        public async Task<QuestionVM?> GetByIdAsync(int id,CancellationToken cancellationToken = default)
        {
            var question = await _unitOfWork.Questions.GetOneAsync(
                q => q.QuestionId == id,
                includes: [q => q.Exam, q => q.Exam.Course, q => q.Choices],
                tracked: false,
                cancellationToken: cancellationToken);

            if (question is null)
                return null;

            return new QuestionVM
            {
                QuestionId = question.QuestionId,
                QuestionText = question.QuestionText,
                QuestionType = question.QuestionType,
                CourseTitle = question.Exam.Course.Title,
                ExamTitle = question.Exam.Title,
                ChoicesCount = question.Choices.Count,

                Choices = question.Choices
                    .OrderBy(c => c.ChoiceId)
                    .Select(c => new ChoiceVM
                    {
                        ChoiceText = c.ChoiceText,
                        IsCorrect = c.IsCorrect
                    })
                    .ToList()
            };
        }
        public async Task<CreateQuestionVM> GetCreateVMAsync()
        {
            var vm = new CreateQuestionVM();

            await LoadDropdownsAsync(vm);

            return vm;
        }
        public async Task<ServiceResult> CreateAsync(CreateQuestionVM vm,CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(vm.QuestionText))
                return ServiceResult.Failure("Question text is required.");

            var course = await _unitOfWork.Courses.GetOneAsync(
                c => c.CourseId == vm.CourseId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (course is null)
                return ServiceResult.Failure("Selected course was not found.");

            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == vm.ExamId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult.Failure("Selected exam was not found.");

            if (exam.CourseId != vm.CourseId)
                return ServiceResult.Failure("Selected exam does not belong to the selected course.");

            var question = new Question
            {
                QuestionText = vm.QuestionText.Trim(),
                QuestionType = vm.QuestionType,
                ExamId = vm.ExamId,
                Choices = new List<Choice>()
            };

            if (vm.QuestionType == QuestionType.MCQ)
            {
                if (vm.Choices.Count != 4)
                    return ServiceResult.Failure("MCQ questions must contain exactly 4 choices.");

                if (vm.Choices.Any(c => string.IsNullOrWhiteSpace(c.ChoiceText)))
                    return ServiceResult.Failure("All choices are required.");

                if (vm.CorrectChoice < 1 || vm.CorrectChoice > 4)
                    return ServiceResult.Failure("Please select the correct answer.");

                for (int i = 0; i < vm.Choices.Count; i++)
                {
                    question.Choices.Add(new Choice
                    {
                        ChoiceText = vm.Choices[i].ChoiceText.Trim(),
                        IsCorrect = vm.CorrectChoice == i + 1
                    });
                }
            }
            else // True / False
            {
                if (vm.CorrectChoice < 1 || vm.CorrectChoice > 2)
                    return ServiceResult.Failure("Please select the correct answer.");

                question.Choices.Add(new Choice
                {
                    ChoiceText = "True",
                    IsCorrect = vm.CorrectChoice == 1
                });

                question.Choices.Add(new Choice
                {
                    ChoiceText = "False",
                    IsCorrect = vm.CorrectChoice == 2
                });
            }

            await _unitOfWork.Questions.CreateAsync(
                question,
                cancellationToken);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to create question.");

            return ServiceResult.SuccessResult("Question created successfully.");
        }
        public async Task<EditQuestionVM?> GetEditVMAsync(int id, CancellationToken cancellationToken = default)
        {
            var question = await _unitOfWork.Questions.GetOneAsync(
                q => q.QuestionId == id,
                includes: [q => q.Exam, q => q.Choices],
                tracked: false,
                cancellationToken: cancellationToken);

            if (question is null)
                return null;

            var vm = new EditQuestionVM
            {
                QuestionId = question.QuestionId,
                QuestionText = question.QuestionText,
                QuestionType = question.QuestionType,
                CourseId = question.Exam.CourseId,
                ExamId = question.ExamId
            };

            vm.Choices = question.Choices
                .OrderBy(c => c.ChoiceId)
                .Select(c => new EditChoiceVM
                {
                    ChoiceId = c.ChoiceId,
                    ChoiceText = c.ChoiceText
                })
                .ToList();

            var correctChoice = question.Choices
                .OrderBy(c => c.ChoiceId)
                .Select((choice, index) => new { choice, index })
                .FirstOrDefault(x => x.choice.IsCorrect);

            if (correctChoice is not null)
                vm.CorrectChoice = correctChoice.index + 1;

            await LoadDropdownsAsync(vm);

            return vm;
        }
        public async Task<ServiceResult> UpdateAsync(EditQuestionVM vm,CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(vm.QuestionText))
                return ServiceResult.Failure("Question text is required.");

            var question = await _unitOfWork.Questions.GetOneAsync(
                q => q.QuestionId == vm.QuestionId,
                includes: [q => q.Choices],
                cancellationToken: cancellationToken);

            if (question is null)
                return ServiceResult.Failure("Question not found.");

            var course = await _unitOfWork.Courses.GetOneAsync(
                c => c.CourseId == vm.CourseId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (course is null)
                return ServiceResult.Failure("Selected course was not found.");

            var exam = await _unitOfWork.Exams.GetOneAsync(
                e => e.ExamId == vm.ExamId,
                tracked: false,
                cancellationToken: cancellationToken);

            if (exam is null)
                return ServiceResult.Failure("Selected exam was not found.");

            if (exam.CourseId != vm.CourseId)
                return ServiceResult.Failure("Selected exam does not belong to the selected course.");

            question.QuestionText = vm.QuestionText.Trim();
            question.QuestionType = vm.QuestionType;
            question.ExamId = vm.ExamId;

            foreach (var choice in question.Choices.ToList())
            {
                _unitOfWork.Choices.Delete(choice);
            }

            if (vm.QuestionType == QuestionType.MCQ)
            {
                if (vm.Choices.Count != 4)
                    return ServiceResult.Failure("MCQ questions must contain exactly 4 choices.");

                if (vm.Choices.Any(c => string.IsNullOrWhiteSpace(c.ChoiceText)))
                    return ServiceResult.Failure("All choices are required.");

                if (vm.CorrectChoice < 1 || vm.CorrectChoice > 4)
                    return ServiceResult.Failure("Please select the correct answer.");

                for (int i = 0; i < vm.Choices.Count; i++)
                {
                    question.Choices.Add(new Choice
                    {
                        ChoiceText = vm.Choices[i].ChoiceText.Trim(),
                        IsCorrect = vm.CorrectChoice == i + 1
                    });
                }
            }
            else
            {
                if (vm.CorrectChoice < 1 || vm.CorrectChoice > 2)
                    return ServiceResult.Failure("Please select the correct answer.");

                question.Choices.Add(new Choice
                {
                    ChoiceText = "True",
                    IsCorrect = vm.CorrectChoice == 1
                });

                question.Choices.Add(new Choice
                {
                    ChoiceText = "False",
                    IsCorrect = vm.CorrectChoice == 2
                });
            }

            _unitOfWork.Questions.Update(question);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to update question.");

            return ServiceResult.SuccessResult("Question updated successfully.");
        }
        public async Task<IEnumerable<SelectListItem>> GetExamsByCourseAsync(int courseId)
        {
            var exams = await _unitOfWork.Exams.GetAsync(
                e => e.CourseId == courseId,
                tracked: false);

            return exams
                .OrderBy(e => e.Title)
                .Select(e => new SelectListItem
                {
                    Value = e.ExamId.ToString(),
                    Text = e.Title
                });
        }
        public async Task<ServiceResult> DeleteAsync(int id,CancellationToken cancellationToken = default)
        {
            var question = await _unitOfWork.Questions.GetOneAsync(
                q => q.QuestionId == id,
                cancellationToken: cancellationToken);

            if (question is null)
                return ServiceResult.Failure("Question not found.");

            _unitOfWork.Questions.Delete(question);

            var rows = await _unitOfWork.CommitAsync(cancellationToken);

            if (rows == 0)
                return ServiceResult.Failure("Failed to delete question.");

            return ServiceResult.SuccessResult("Question deleted successfully.");
        }
        public async Task LoadDropdownsAsync(CreateQuestionVM vm)
        {
            var courses = await _unitOfWork.Courses.GetAsync(
                tracked: false);

            vm.Courses = courses
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.Title
                });

            if (vm.CourseId > 0)
            {
                var exams = await _unitOfWork.Exams.GetAsync(
                    e => e.CourseId == vm.CourseId,
                    tracked: false);

                vm.Exams = exams
                    .OrderBy(e => e.Title)
                    .Select(e => new SelectListItem
                    {
                        Value = e.ExamId.ToString(),
                        Text = e.Title
                    });
            }
            else
            {
                vm.Exams = Enumerable.Empty<SelectListItem>();
            }
        }

        public async Task LoadDropdownsAsync(EditQuestionVM vm)
        {
            var courses = await _unitOfWork.Courses.GetAsync(
                tracked: false);

            vm.Courses = courses
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.Title
                });

            if (vm.CourseId > 0)
            {
                var exams = await _unitOfWork.Exams.GetAsync(
                    e => e.CourseId == vm.CourseId,
                    tracked: false);

                vm.Exams = exams
                    .OrderBy(e => e.Title)
                    .Select(e => new SelectListItem
                    {
                        Value = e.ExamId.ToString(),
                        Text = e.Title
                    });
            }
            else
            {
                vm.Exams = Enumerable.Empty<SelectListItem>();
            }
        }
    }
}
