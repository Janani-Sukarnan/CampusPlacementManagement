
using CampusPlacementManagement.DTOs.JobOpenings;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IJobOpeningService
    {
        Task<IEnumerable<JobOpeningResponse>> GetAllAsync();

        Task<IEnumerable<JobOpeningResponse>> GetOpenAsync();

        Task<IEnumerable<JobOpeningResponse>> GetByCompanyHRAsync(
            string userId);

        Task<JobOpeningResponse?> GetByIdAsync(int id);

        Task<JobOpeningResponse> CreateAsync(
            CreateJobOpeningRequest request,
            string userId);

        Task<JobOpeningResponse?> UpdateAsync(
            int id,
            UpdateJobOpeningRequest request,
            string userId,
            bool isSuperAdmin);
    }
}
