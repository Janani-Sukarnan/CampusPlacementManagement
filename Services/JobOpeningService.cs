
using CampusPlacementManagement.DTOs.JobOpenings;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services
{
    public class JobOpeningService : IJobOpeningService
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly ICompanyHRRepository _companyHRRepository;
        private readonly ICompanyRepository _companyRepository;

        public JobOpeningService(
            IJobOpeningRepository jobOpeningRepository,
            ICompanyHRRepository companyHRRepository,
            ICompanyRepository companyRepository)
        {
            _jobOpeningRepository = jobOpeningRepository;
            _companyHRRepository = companyHRRepository;
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<JobOpeningResponse>> GetAllAsync()
        {
            var jobs = await _jobOpeningRepository.GetAllAsync();
            return jobs.Select(MapToResponse);
        }

        public async Task<IEnumerable<JobOpeningResponse>> GetOpenAsync()
        {
            var jobs = await _jobOpeningRepository.GetOpenAsync();
            return jobs.Select(MapToResponse);
        }

        public async Task<IEnumerable<JobOpeningResponse>> GetByCompanyHRAsync(
            string userId)
        {
            var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                throw new InvalidOperationException(
                    "Company HR profile not found.");

            var jobs = await _jobOpeningRepository
                .GetByCompanyIdAsync(companyHR.CompanyId);

            return jobs.Select(MapToResponse);
        }

        public async Task<JobOpeningResponse?> GetByIdAsync(int id)
        {
            var job = await _jobOpeningRepository.GetByIdAsync(id);

            return job == null ? null : MapToResponse(job);
        }

        public async Task<JobOpeningResponse> CreateAsync(
            CreateJobOpeningRequest request,
            string userId)
        {
            var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                throw new InvalidOperationException(
                    "Company HR profile not found.");

            var company = await _companyRepository
                .GetCompanyByIdAsync(companyHR.CompanyId);

            if (company == null ||
                company.Status != CompanyStatus.Approved)
                throw new InvalidOperationException(
                    "Only approved companies can post job openings.");

            if (request.ApplicationDeadline <= DateTime.UtcNow)
                throw new InvalidOperationException(
                    "Application deadline must be in the future.");

            var job = new JobOpening
            {
                CompanyId = companyHR.CompanyId,
                JobTitle = request.JobTitle,
                JobDescription = request.JobDescription,
                Location = request.Location,
                EmploymentType = request.EmploymentType,
                MinimumCGPA = request.MinimumCGPA,
                SalaryRange = request.SalaryRange,
                ApplicationDeadline = request.ApplicationDeadline,
                Status = JobOpeningStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            var savedJob = await _jobOpeningRepository.AddAsync(job);

            var result = await _jobOpeningRepository
                .GetByIdAsync(savedJob.JobOpeningId);

            return MapToResponse(result!);
        }

        public async Task<JobOpeningResponse?> UpdateAsync(
            int id,
            UpdateJobOpeningRequest request,
            string userId,
            bool isSuperAdmin)
        {
            var job = await _jobOpeningRepository.GetByIdAsync(id);

            if (job == null)
                return null;

            if (!isSuperAdmin)
            {
                var companyHR = await _companyHRRepository
                    .GetByUserIdAsync(userId);

                if (companyHR == null ||
                    companyHR.CompanyId != job.CompanyId)
                    throw new UnauthorizedAccessException(
                        "You cannot update another company's job opening.");
            }

            if (request.Status == JobOpeningStatus.Open &&
                request.ApplicationDeadline <= DateTime.UtcNow)
                throw new InvalidOperationException(
                    "An expired job opening cannot be reopened.");

            job.JobTitle = request.JobTitle;
            job.JobDescription = request.JobDescription;
            job.Location = request.Location;
            job.EmploymentType = request.EmploymentType;
            job.MinimumCGPA = request.MinimumCGPA;
            job.SalaryRange = request.SalaryRange;
            job.ApplicationDeadline = request.ApplicationDeadline;
            job.Status = request.Status;
            job.UpdatedAt = DateTime.UtcNow;

            var updatedJob = await _jobOpeningRepository.UpdateAsync(job);

            return MapToResponse(updatedJob);
        }

        private static JobOpeningResponse MapToResponse(JobOpening job)
        {
            return new JobOpeningResponse
            {
                JobOpeningId = job.JobOpeningId,
                CompanyId = job.CompanyId,
                CompanyName = job.Company?.CompanyName ?? string.Empty,
                JobTitle = job.JobTitle,
                JobDescription = job.JobDescription,
                Location = job.Location,
                EmploymentType = job.EmploymentType,
                MinimumCGPA = job.MinimumCGPA,
                SalaryRange = job.SalaryRange,
                ApplicationDeadline = job.ApplicationDeadline,
                Status = job.Status,
                CreatedAt = job.CreatedAt
            };
        }
    }
}
