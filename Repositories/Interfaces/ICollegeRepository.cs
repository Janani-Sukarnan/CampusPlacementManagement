
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICollegeRepository
    {
        Task<IEnumerable<College>> GetAllCollegesAsync();

        Task<College?> GetCollegeByIdAsync(int collegeId);

        Task<College?> GetCollegeByCodeAsync(string collegeCode);

        Task<College> AddCollegeAsync(College college);

        Task<College> UpdateCollegeAsync(College college);

        Task<bool> DeleteCollegeAsync(int collegeId);

        Task<IEnumerable<College>> SearchCollegesAsync(string searchTerm);
    }
}
