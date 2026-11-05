using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICompanyHRRepository
    {
        Task<IEnumerable<CompanyHR>> GetAllAsync();

        Task<CompanyHR?> GetByIdAsync(int id);

        Task<CompanyHR?> GetByUserIdAsync(string userId);

        Task<IEnumerable<CompanyHR>> GetPendingAsync();

        Task AddAsync(CompanyHR companyHR);

        Task UpdateAsync(CompanyHR companyHR);

        Task<bool> ExistsAsync(int id);
    }
}