using CampusPlacementManagement.DTOs.Interviews;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _repository;
        private readonly IJobApplicationRepository _applicationRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICompanyHRRepository _companyHRRepository;

        public InterviewService(
            IInterviewRepository repository,
            IJobApplicationRepository applicationRepository,
            IStudentRepository studentRepository,
            ICompanyHRRepository companyHRRepository)
        {
            _repository = repository;
            _applicationRepository = applicationRepository;
            _studentRepository = studentRepository;
            _companyHRRepository = companyHRRepository;
        }

        public async Task<IEnumerable<InterviewResponse>> GetAllAsync()
        {
            var interviews = await _repository.GetAllAsync();
            return interviews.Select(MapToResponse);
        }

        public async Task<IEnumerable<InterviewResponse>> GetMyInterviewsAsync(
            string userId)
        {
            var student = await _studentRepository.GetByUserIdAsync(userId);

            if (student == null)
                throw new InvalidOperationException(
                    "Student profile not found.");

            var interviews = await _repository.GetByStudentIdAsync(
                student.StudentId);

            return interviews.Select(MapToResponse);
        }

        public async Task<IEnumerable<InterviewResponse>>
            GetCompanyInterviewsAsync(string userId)
        {
            var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                throw new InvalidOperationException(
                    "Company HR profile not found.");

            var interviews = await _repository.GetByCompanyIdAsync(
                companyHR.CompanyId);

            return interviews.Select(MapToResponse);
        }

        public async Task<InterviewResponse?> GetByIdAsync(int id)
        {
            var interview = await _repository.GetByIdAsync(id);

            return interview == null
                ? null
                : MapToResponse(interview);
        }

        public async Task<InterviewResponse> ScheduleAsync(
            CreateInterviewRequest request,
            string userId)
        {
            var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                throw new InvalidOperationException(
                    "Company HR profile not found.");

            var application = await _applicationRepository.GetByIdAsync(
                request.JobApplicationId);

            if (application == null)
                throw new InvalidOperationException(
                    "Job application not found.");

            if (application.JobOpening!.CompanyId != companyHR.CompanyId)
                throw new UnauthorizedAccessException(
                    "You can only schedule interviews for your company's applications.");

            if (request.InterviewDate <= DateTime.UtcNow)
                throw new InvalidOperationException(
                    "Interview date must be in the future.");

            if (application.Status == ApplicationStatus.Rejected ||
                application.Status == ApplicationStatus.Withdrawn)
                throw new InvalidOperationException(
                    "An interview cannot be scheduled for a rejected or withdrawn application.");

            var interview = new Interview
            {
                JobApplicationId = request.JobApplicationId,
                InterviewDate = request.InterviewDate,
                InterviewRound = request.InterviewRound,
                InterviewMode = request.InterviewMode,
                Location = request.Location,
                MeetingLink = request.MeetingLink,
                Status = InterviewStatus.Scheduled,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(interview);

            var savedInterview = await _repository.GetByIdAsync(
                interview.InterviewId);

            return MapToResponse(savedInterview!);
        }

        public async Task<InterviewResponse> UpdateStatusAsync(
            int id,
            UpdateInterviewStatusRequest request,
            string userId,
            bool isSuperAdmin)
        {
            var interview = await _repository.GetByIdAsync(id);

            if (interview == null)
                throw new InvalidOperationException(
                    "Interview not found.");

            if (!isSuperAdmin)
            {
                var companyHR =
                    await _companyHRRepository.GetByUserIdAsync(userId);

                if (companyHR == null ||
                    companyHR.CompanyId !=
                    interview.JobApplication!.JobOpening!.CompanyId)
                    throw new UnauthorizedAccessException(
                        "You can only update interviews for your company's applications.");
            }

            interview.Status = request.Status;
            interview.Feedback = request.Feedback;

            await _repository.UpdateAsync(interview);

            return MapToResponse(interview);
        }

        private static InterviewResponse MapToResponse(Interview interview)
        {
            var application = interview.JobApplication;

            return new InterviewResponse
            {
                InterviewId = interview.InterviewId,
                JobApplicationId = interview.JobApplicationId,
                StudentId = application?.StudentId ?? 0,
                StudentName = application?.Student?.User?.FullName
                    ?? string.Empty,
                JobTitle = application?.JobOpening?.JobTitle
                    ?? string.Empty,
                CompanyName = application?.JobOpening?.Company?.CompanyName
                    ?? string.Empty,
                InterviewDate = interview.InterviewDate,
                InterviewRound = interview.InterviewRound,
                InterviewMode = interview.InterviewMode,
                Location = interview.Location,
                MeetingLink = interview.MeetingLink,
                Feedback = interview.Feedback,
                Status = interview.Status.ToString(),
                CreatedAt = interview.CreatedAt
            };
        }
    }
}