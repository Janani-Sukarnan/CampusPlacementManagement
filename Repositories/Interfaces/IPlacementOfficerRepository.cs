
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IPlacementOfficerRepository
    {
        Task<IEnumerable<PlacementOfficer>> GetAllAsync();

        Task<IEnumerable<PlacementOfficer>> GetPendingAsync();

        Task<PlacementOfficer?> GetByIdAsync(int id);

        Task<PlacementOfficer?> GetByUserIdAsync(string userId);

        Task<PlacementOfficer> AddAsync(PlacementOfficer officer);

        Task<PlacementOfficer> UpdateAsync(PlacementOfficer officer);
    }
}
