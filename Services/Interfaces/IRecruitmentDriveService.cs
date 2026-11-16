
using CampusPlacementManagement.DTOs.RecruitmentDrives;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IRecruitmentDriveService
    {
        Task<IEnumerable<RecruitmentDriveResponse>> GetAllAsync();

        Task<IEnumerable<RecruitmentDriveResponse>> GetByCompanyHRAsync(
            string userId);

        Task<RecruitmentDriveResponse?> GetByIdAsync(int id);

        Task<RecruitmentDriveResponse> CreateAsync(
            CreateRecruitmentDriveRequest request,
            string userId);

        Task<RecruitmentDriveResponse?> UpdateAsync(
            int id,
            UpdateRecruitmentDriveRequest request,
            string userId,
            bool isSuperAdmin);
    }
}
