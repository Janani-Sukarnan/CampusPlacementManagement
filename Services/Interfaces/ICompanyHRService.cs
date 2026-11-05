using CampusPlacementManagement.DTOs.CompanyHRs;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICompanyHRService
    {
        Task<IEnumerable<CompanyHRResponse>> GetAllAsync();

        Task<CompanyHRResponse?> GetByIdAsync(int id);

        Task<CompanyHRResponse?> GetByUserIdAsync(string userId);

        Task<IEnumerable<CompanyHRResponse>> GetPendingAsync();

        Task<CompanyHRResponse> RegisterAsync(
            CreateCompanyHRRequest request);

        Task<bool> ApproveAsync(int id, string approvedBy);

        Task<bool> RejectAsync(int id);
    }
}