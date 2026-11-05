using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();

        Task<IEnumerable<Company>> GetPendingCompaniesAsync();

        Task<Company?> GetCompanyByIdAsync(int companyId);

        Task<Company?> GetCompanyByEmailAsync(string companyEmail);

        Task<Company> AddCompanyAsync(Company company);

        Task<Company> UpdateCompanyAsync(Company company);
    }
}