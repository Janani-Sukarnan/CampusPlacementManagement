
using CampusPlacementManagement.DTOs.JobApplications;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IJobApplicationService
    {
        Task<IEnumerable<JobApplicationResponse>> GetAllAsync();

        Task<IEnumerable<JobApplicationResponse>> GetMyApplicationsAsync(
            string userId);

        Task<IEnumerable<JobApplicationResponse>> GetCompanyApplicationsAsync(
            string userId);

        Task<JobApplicationResponse?> GetByIdAsync(int id);

        Task<JobApplicationResponse> ApplyAsync(
            CreateJobApplicationRequest request,
            string userId);

        Task<JobApplicationResponse> UpdateStatusAsync(
            int id,
            UpdateJobApplicationStatusRequest request,
            string userId,
            bool isSuperAdmin);
    }
}
