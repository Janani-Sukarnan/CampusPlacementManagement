
using CampusPlacementManagement.DTOs.RecruitmentDrives;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services
{
    public class RecruitmentDriveService : IRecruitmentDriveService
    {
        private readonly IRecruitmentDriveRepository _recruitmentDriveRepository;
        private readonly ICompanyHRRepository _companyHRRepository;
        private readonly ICompanyRepository _companyRepository;

        public RecruitmentDriveService(
            IRecruitmentDriveRepository recruitmentDriveRepository,
            ICompanyHRRepository companyHRRepository,
            ICompanyRepository companyRepository)
        {
            _recruitmentDriveRepository = recruitmentDriveRepository;
            _companyHRRepository = companyHRRepository;
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<RecruitmentDriveResponse>> GetAllAsync()
        {
            var drives = await _recruitmentDriveRepository.GetAllAsync();
            return drives.Select(MapToResponse);
        }

        public async Task<IEnumerable<RecruitmentDriveResponse>>
            GetByCompanyHRAsync(string userId)
        {
            var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                throw new InvalidOperationException(
                    "Company HR profile not found.");

            var drives = await _recruitmentDriveRepository
                .GetByCompanyIdAsync(companyHR.CompanyId);

            return drives.Select(MapToResponse);
        }

        public async Task<RecruitmentDriveResponse?> GetByIdAsync(int id)
        {
            var drive = await _recruitmentDriveRepository.GetByIdAsync(id);

            return drive == null ? null : MapToResponse(drive);
        }

        public async Task<RecruitmentDriveResponse> CreateAsync(
            CreateRecruitmentDriveRequest request,
            string userId)
        {
            var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                throw new InvalidOperationException(
                    "Company HR profile not found.");

            var company = await _companyRepository
                .GetCompanyByIdAsync(companyHR.CompanyId);

            if (company == null || company.Status != CompanyStatus.Approved)
                throw new InvalidOperationException(
                    "Only approved companies can create recruitment drives.");

            if (request.StartDate < DateTime.UtcNow)
                throw new InvalidOperationException(
                    "Recruitment drive start date must be in the future.");

            if (request.EndDate < request.StartDate)
                throw new InvalidOperationException(
                    "End date cannot be earlier than start date.");

            var drive = new RecruitmentDrive
            {
                CompanyId = companyHR.CompanyId,
                DriveName = request.DriveName,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Venue = request.Venue,
                CreatedAt = DateTime.UtcNow
            };

            var savedDrive =
                await _recruitmentDriveRepository.AddAsync(drive);

            var result = await _recruitmentDriveRepository
                .GetByIdAsync(savedDrive.RecruitmentDriveId);

            return MapToResponse(result!);
        }

        public async Task<RecruitmentDriveResponse?> UpdateAsync(
            int id,
            UpdateRecruitmentDriveRequest request,
            string userId,
            bool isSuperAdmin)
        {
            var drive = await _recruitmentDriveRepository.GetByIdAsync(id);

            if (drive == null)
                return null;

            if (!isSuperAdmin)
            {
                var companyHR =
                    await _companyHRRepository.GetByUserIdAsync(userId);

                if (companyHR == null ||
                    companyHR.CompanyId != drive.CompanyId)
                    throw new UnauthorizedAccessException(
                        "You cannot update another company's recruitment drive.");
            }

            if (request.StartDate < DateTime.UtcNow)
                throw new InvalidOperationException(
                    "Recruitment drive start date must be in the future.");

            if (request.EndDate < request.StartDate)
                throw new InvalidOperationException(
                    "End date cannot be earlier than start date.");

            drive.DriveName = request.DriveName;
            drive.Description = request.Description;
            drive.StartDate = request.StartDate;
            drive.EndDate = request.EndDate;
            drive.Venue = request.Venue;

            var updatedDrive =
                await _recruitmentDriveRepository.UpdateAsync(drive);

            return MapToResponse(updatedDrive);
        }

        private static RecruitmentDriveResponse MapToResponse(
            RecruitmentDrive drive)
        {
            return new RecruitmentDriveResponse
            {
                RecruitmentDriveId = drive.RecruitmentDriveId,
                CompanyId = drive.CompanyId,
                CompanyName = drive.Company?.CompanyName ?? string.Empty,
                DriveName = drive.DriveName,
                Description = drive.Description,
                StartDate = drive.StartDate,
                EndDate = drive.EndDate,
                Venue = drive.Venue,
                CreatedAt = drive.CreatedAt
            };
        }
    }
}
