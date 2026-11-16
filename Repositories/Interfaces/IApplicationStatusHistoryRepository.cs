using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IApplicationStatusHistoryRepository
    {
        Task<IEnumerable<ApplicationStatusHistory>> GetAllAsync();

        Task<IEnumerable<ApplicationStatusHistory>> GetByJobApplicationIdAsync(
            int jobApplicationId);

        Task AddAsync(ApplicationStatusHistory history);
    }
}