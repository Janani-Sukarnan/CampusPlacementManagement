
using CampusPlacementManagement.DTOs.JobTargetColleges;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services
{
    public class JobTargetCollegeService : IJobTargetCollegeService
    {
        private readonly IJobTargetCollegeRepository _repository;
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly ICompanyHRRepository _companyHRRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICollegeRepository _collegeRepository;

        public JobTargetCollegeService(
            IJobTargetCollegeRepository repository,
            IJobOpeningRepository jobOpeningRepository,
            ICompanyHRRepository companyHRRepository,
            ICompanyRepository companyRepository,
            ICollegeRepository collegeRepository)
        {
            _repository = repository;
            _jobOpeningRepository = jobOpeningRepository;
            _companyHRRepository = companyHRRepository;
            _companyRepository = companyRepository;
            _collegeRepository = collegeRepository;
        }

        public async Task<IEnumerable<JobTargetCollegeResponse>> GetAllAsync()
        {
            var targets = await _repository.GetAllAsync();
            return targets.Select(MapToResponse);
        }

        public async Task<JobTargetCollegeResponse?> GetByIdAsync(int id)
        {
            var target = await _repository.GetByIdAsync(id);
            return target == null ? null : MapToResponse(target);
        }

        public async Task<IEnumerable<JobTargetCollegeResponse>>
            GetMyCompanyTargetsAsync(string userId)
        {
            var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                throw new InvalidOperationException("Company HR profile not found.");

            var targets = await _repository.GetByCompanyIdAsync(companyHR.CompanyId);
            return targets.Select(MapToResponse);
        }

        public async Task<JobTargetCollegeResponse> CreateAsync(
            CreateJobTargetCollegeRequest request,
            string userId)
        {
            var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                throw new InvalidOperationException("Company HR profile not found.");

            var company = await _companyRepository.GetCompanyByIdAsync(
                companyHR.CompanyId);

            if (company == null || company.Status != CompanyStatus.Approved)
                throw new InvalidOperationException(
                    "Only approved companies can target colleges.");

            var job = await _jobOpeningRepository.GetByIdAsync(
                request.JobOpeningId);

            if (job == null)
                throw new InvalidOperationException("Job opening not found.");

            if (job.CompanyId != companyHR.CompanyId)
                throw new UnauthorizedAccessException(
                    "You can only target colleges for your own job openings.");

            if (job.Status != JobOpeningStatus.Open ||
                job.ApplicationDeadline <= DateTime.UtcNow)
                throw new InvalidOperationException(
                    "Only open job openings with a future deadline can target colleges.");

            var college = await _collegeRepository.GetCollegeByIdAsync(
    request.CollegeId);

            if (college == null)
                throw new InvalidOperationException("College not found.");

            if (college.Status != CollegeStatus.Approved)
                throw new InvalidOperationException(
                    "You can only target approved colleges.");

            if (await _repository.ExistsAsync(
                request.JobOpeningId, request.CollegeId))
                throw new InvalidOperationException(
                    "This college is already targeted for this job opening.");

            var target = new JobTargetCollege
            {
                JobOpeningId = request.JobOpeningId,
                CollegeId = request.CollegeId,
                TargetedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(target);

            var savedTarget = await _repository.GetByIdAsync(
                target.JobTargetCollegeId);

            return MapToResponse(savedTarget!);
        }

        private static JobTargetCollegeResponse MapToResponse(
            JobTargetCollege target)
        {
            return new JobTargetCollegeResponse
            {
                JobTargetCollegeId = target.JobTargetCollegeId,
                JobOpeningId = target.JobOpeningId,
                JobTitle = target.JobOpening.JobTitle,
                CollegeId = target.CollegeId,
                CollegeName = target.College.CollegeName,
                TargetedAt = target.TargetedAt
            };
        }
    }
}
