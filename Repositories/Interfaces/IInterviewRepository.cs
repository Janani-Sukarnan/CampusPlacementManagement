using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IInterviewRepository
    {
        Task<IEnumerable<Interview>> GetAllAsync();

        Task<IEnumerable<Interview>> GetByStudentIdAsync(int studentId);

        Task<IEnumerable<Interview>> GetByCompanyIdAsync(int companyId);

        Task<Interview?> GetByIdAsync(int id);

        Task AddAsync(Interview interview);

        Task UpdateAsync(Interview interview);
    }
}