using CampusPlacementManagement.DTOs.Companies;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponse> RegisterCompanyAsync(CreateCompanyRequest request);

        Task<IEnumerable<CompanyResponse>> GetAllCompaniesAsync();

        Task<IEnumerable<CompanyResponse>> GetPendingCompaniesAsync();

        Task<CompanyResponse?> GetCompanyByIdAsync(int companyId);

        Task<CompanyResponse> UpdateCompanyAsync(
            int companyId,
            UpdateCompanyRequest request);

        Task<bool> ApproveCompanyAsync(int companyId);

        Task<bool> RejectCompanyAsync(int companyId);

        Task<bool> SuspendCompanyAsync(int companyId);
    }
}