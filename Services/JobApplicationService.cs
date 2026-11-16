using CampusPlacementManagement.DTOs.JobApplications;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _repository;
        private readonly IStudentRepository _studentRepository;
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly ICompanyHRRepository _companyHRRepository;
        private readonly IApplicationStatusHistoryRepository _historyRepository;

        public JobApplicationService(
            IJobApplicationRepository repository,
            IStudentRepository studentRepository,
            IJobOpeningRepository jobOpeningRepository,
            ICompanyHRRepository companyHRRepository,
            IApplicationStatusHistoryRepository historyRepository)
        {
            _repository = repository;
            _studentRepository = studentRepository;
            _jobOpeningRepository = jobOpeningRepository;
            _companyHRRepository = companyHRRepository;
            _historyRepository = historyRepository;
        }

        public async Task<IEnumerable<JobApplicationResponse>> GetAllAsync()
        {
            var applications = await _repository.GetAllAsync();
            return applications.Select(MapToResponse);
        }

        public async Task<IEnumerable<JobApplicationResponse>>
            GetMyApplicationsAsync(string userId)
        {
            var student = await _studentRepository.GetByUserIdAsync(userId);

            if (student == null)
                throw new InvalidOperationException(
                    "Student profile not found.");

            var applications = await _repository.GetByStudentIdAsync(
                student.StudentId);

            return applications.Select(MapToResponse);
        }

        public async Task<IEnumerable<JobApplicationResponse>>
            GetCompanyApplicationsAsync(string userId)
        {
            var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                throw new InvalidOperationException(
                    "Company HR profile not found.");

            var applications = await _repository.GetByCompanyIdAsync(
                companyHR.CompanyId);

            return applications.Select(MapToResponse);
        }

        public async Task<JobApplicationResponse?> GetByIdAsync(int id)
        {
            var application = await _repository.GetByIdAsync(id);

            return application == null
                ? null
                : MapToResponse(application);
        }

        public async Task<JobApplicationResponse> ApplyAsync(
            CreateJobApplicationRequest request,
            string userId)
        {
            var student = await _studentRepository.GetByUserIdAsync(userId);

            if (student == null)
                throw new InvalidOperationException(
                    "Student profile not found.");

            if (student.User?.AccountStatus != AccountStatus.Approved)
                throw new InvalidOperationException(
                    "Your student account must be approved to apply.");

            var job = await _jobOpeningRepository.GetByIdAsync(
                request.JobOpeningId);

            if (job == null)
                throw new InvalidOperationException(
                    "Job opening not found.");

            if (job.Status != JobOpeningStatus.Open ||
                job.ApplicationDeadline <= DateTime.UtcNow)
                throw new InvalidOperationException(
                    "Applications are closed for this job opening.");

            if (student.CGPA < job.MinimumCGPA)
                throw new InvalidOperationException(
                    "Your CGPA does not meet the minimum requirement.");

            if (await _repository.ExistsAsync(
                student.StudentId, job.JobOpeningId))
                throw new InvalidOperationException(
                    "You have already applied for this job.");

            var application = new JobApplication
            {
                StudentId = student.StudentId,
                JobOpeningId = job.JobOpeningId,
                AppliedAt = DateTime.UtcNow,
                Status = ApplicationStatus.Applied,
                ResumeUrl = request.ResumeUrl ?? student.ResumeUrl,
                CoverLetter = request.CoverLetter
            };

            await _repository.AddAsync(application);

            var savedApplication = await _repository.GetByIdAsync(
                application.JobApplicationId);

            return MapToResponse(savedApplication!);
        }

        public async Task<JobApplicationResponse> UpdateStatusAsync(
            int id,
            UpdateJobApplicationStatusRequest request,
            string userId,
            bool isSuperAdmin)
        {
            var application = await _repository.GetByIdAsync(id);

            if (application == null)
                throw new InvalidOperationException(
                    "Job application not found.");

            if (!isSuperAdmin)
            {
                var companyHR =
                    await _companyHRRepository.GetByUserIdAsync(userId);

                if (companyHR == null ||
                    companyHR.CompanyId != application.JobOpening!.CompanyId)
                    throw new UnauthorizedAccessException(
                        "You can only update applications for your company's jobs.");
            }

            var previousStatus = application.Status;

            if (previousStatus != request.Status)
            {
                application.Status = request.Status;

                await _repository.UpdateAsync(application);

                var history = new ApplicationStatusHistory
                {
                    JobApplicationId = application.JobApplicationId,
                    PreviousStatus = previousStatus,
                    NewStatus = request.Status,
                    ChangedAt = DateTime.UtcNow,
                    ChangedByUserId = userId
                };

                await _historyRepository.AddAsync(history);
            }

            var updatedApplication = await _repository.GetByIdAsync(id);

            return MapToResponse(updatedApplication!);
        }

        private static JobApplicationResponse MapToResponse(
            JobApplication application)
        {
            return new JobApplicationResponse
            {
                JobApplicationId = application.JobApplicationId,
                StudentId = application.StudentId,
                StudentName = application.Student?.User?.FullName
                    ?? string.Empty,
                JobOpeningId = application.JobOpeningId,
                JobTitle = application.JobOpening?.JobTitle
                    ?? string.Empty,
                CompanyName = application.JobOpening?.Company?.CompanyName
                    ?? string.Empty,
                AppliedAt = application.AppliedAt,
                Status = application.Status.ToString(),
                ResumeUrl = application.ResumeUrl,
                CoverLetter = application.CoverLetter
            };
        }
    }
}