
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IJobOpeningRepository
    {
        Task<IEnumerable<JobOpening>> GetAllAsync();

        Task<IEnumerable<JobOpening>> GetByCompanyIdAsync(int companyId);

        Task<IEnumerable<JobOpening>> GetOpenAsync();

        Task<JobOpening?> GetByIdAsync(int id);

        Task<JobOpening> AddAsync(JobOpening jobOpening);

        Task<JobOpening> UpdateAsync(JobOpening jobOpening);
    }
}
