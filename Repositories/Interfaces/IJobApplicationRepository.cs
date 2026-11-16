
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IJobApplicationRepository
    {
        Task<IEnumerable<JobApplication>> GetAllAsync();

        Task<IEnumerable<JobApplication>> GetByStudentIdAsync(int studentId);

        Task<IEnumerable<JobApplication>> GetByCompanyIdAsync(int companyId);

        Task<JobApplication?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int studentId, int jobOpeningId);

        Task AddAsync(JobApplication jobApplication);

        Task UpdateAsync(JobApplication jobApplication);
    }
}
