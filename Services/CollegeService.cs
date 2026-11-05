
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services
{
    public class CollegeService : ICollegeService
    {
        private readonly ICollegeRepository _collegeRepository;

        public CollegeService(ICollegeRepository collegeRepository)
        {
            _collegeRepository = collegeRepository;
        }

        public async Task<IEnumerable<CollegeResponse>> GetAllCollegesAsync()
        {
            var colleges = await _collegeRepository.GetAllCollegesAsync();

            return colleges.Select(MapToResponse);
        }

        public async Task<IEnumerable<CollegeResponse>> GetPendingCollegesAsync()
        {
            var colleges = await _collegeRepository.GetAllCollegesAsync();

            return colleges
                .Where(c => c.Status == CollegeStatus.Pending)
                .Select(MapToResponse);
        }

        public async Task<CollegeResponse?> GetCollegeByIdAsync(int collegeId)
        {
            var college = await _collegeRepository.GetCollegeByIdAsync(collegeId);

            return college == null ? null : MapToResponse(college);
        }

        public async Task<CollegeResponse> CreateCollegeAsync(
            CreateCollegeRequest request)
        {
            var existingCollege =
                await _collegeRepository.GetCollegeByCodeAsync(request.CollegeCode);

            if (existingCollege != null)
            {
                throw new InvalidOperationException(
                    "A college with this college code already exists.");
            }

            var college = new College
            {
                CollegeName = request.CollegeName,
                CollegeCode = request.CollegeCode,
                UniversityName = request.UniversityName,
                OfficialEmail = request.OfficialEmail,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Website = request.Website,
                Status = CollegeStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var createdCollege =
                await _collegeRepository.AddCollegeAsync(college);

            return MapToResponse(createdCollege);
        }

        public async Task<CollegeResponse?> UpdateCollegeAsync(
            int collegeId,
            UpdateCollegeRequest request)
        {
            var college =
                await _collegeRepository.GetCollegeByIdAsync(collegeId);

            if (college == null)
            {
                return null;
            }

            var existingCollege =
                await _collegeRepository.GetCollegeByCodeAsync(request.CollegeCode);

            if (existingCollege != null &&
                existingCollege.CollegeId != collegeId)
            {
                throw new InvalidOperationException(
                    "Another college already uses this college code.");
            }

            college.CollegeName = request.CollegeName;
            college.CollegeCode = request.CollegeCode;
            college.UniversityName = request.UniversityName;
            college.OfficialEmail = request.OfficialEmail;
            college.PhoneNumber = request.PhoneNumber;
            college.Address = request.Address;
            college.Website = request.Website;

            var updatedCollege =
                await _collegeRepository.UpdateCollegeAsync(college);

            return MapToResponse(updatedCollege);
        }

        public async Task<bool> ApproveCollegeAsync(int collegeId)
        {
            var college =
                await _collegeRepository.GetCollegeByIdAsync(collegeId);

            if (college == null)
            {
                return false;
            }

            college.Status = CollegeStatus.Approved;

            await _collegeRepository.UpdateCollegeAsync(college);

            return true;
        }

        public async Task<bool> RejectCollegeAsync(int collegeId)
        {
            var college =
                await _collegeRepository.GetCollegeByIdAsync(collegeId);

            if (college == null)
            {
                return false;
            }

            college.Status = CollegeStatus.Rejected;

            await _collegeRepository.UpdateCollegeAsync(college);

            return true;
        }

        public async Task<bool> SuspendCollegeAsync(int collegeId)
        {
            var college =
                await _collegeRepository.GetCollegeByIdAsync(collegeId);

            if (college == null)
            {
                return false;
            }

            college.Status = CollegeStatus.Suspended;

            await _collegeRepository.UpdateCollegeAsync(college);

            return true;
        }

        private static CollegeResponse MapToResponse(College college)
        {
            return new CollegeResponse
            {
                CollegeId = college.CollegeId,
                CollegeName = college.CollegeName,
                CollegeCode = college.CollegeCode,
                UniversityName = college.UniversityName,
                OfficialEmail = college.OfficialEmail,
                PhoneNumber = college.PhoneNumber,
                Address = college.Address,
                Website = college.Website,
                Status = college.Status,
                CreatedAt = college.CreatedAt
            };
        }
    }
}
