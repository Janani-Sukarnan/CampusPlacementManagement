using CampusPlacementManagement.DTOs.Colleges;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICollegeService
    {
        Task<IEnumerable<CollegeResponse>> GetAllCollegesAsync();

        Task<IEnumerable<CollegeResponse>> GetPendingCollegesAsync();

        Task<CollegeResponse?> GetCollegeByIdAsync(int collegeId);

        Task<CollegeResponse> CreateCollegeAsync(CreateCollegeRequest request);

        Task<CollegeResponse?> UpdateCollegeAsync(
            int collegeId,
            UpdateCollegeRequest request);

        Task<bool> ApproveCollegeAsync(int collegeId);

        Task<bool> RejectCollegeAsync(int collegeId);

        Task<bool> SuspendCollegeAsync(int collegeId);
    }
}