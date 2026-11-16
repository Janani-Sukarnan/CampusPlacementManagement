
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IJobTargetCollegeRepository
    {
        Task<IEnumerable<JobTargetCollege>> GetAllAsync();

        Task<JobTargetCollege?> GetByIdAsync(int id);

        Task<IEnumerable<JobTargetCollege>> GetByCompanyIdAsync(int companyId);

        Task<bool> ExistsAsync(int jobOpeningId, int collegeId);

        Task AddAsync(JobTargetCollege jobTargetCollege);
    }
}
