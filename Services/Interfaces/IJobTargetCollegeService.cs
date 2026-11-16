
using CampusPlacementManagement.DTOs.JobTargetColleges;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IJobTargetCollegeService
    {
        Task<IEnumerable<JobTargetCollegeResponse>> GetAllAsync();

        Task<JobTargetCollegeResponse?> GetByIdAsync(int id);

        Task<IEnumerable<JobTargetCollegeResponse>> GetMyCompanyTargetsAsync(
            string userId);

        Task<JobTargetCollegeResponse> CreateAsync(
            CreateJobTargetCollegeRequest request,
            string userId);
    }
}
