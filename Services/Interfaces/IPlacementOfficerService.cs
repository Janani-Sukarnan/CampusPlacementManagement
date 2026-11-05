
using CampusPlacementManagement.DTOs.PlacementOfficers;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IPlacementOfficerService
    {
        Task<IEnumerable<PlacementOfficerResponse>> GetAllAsync();

        Task<IEnumerable<PlacementOfficerResponse>> GetPendingAsync();

        Task<PlacementOfficerResponse?> GetByIdAsync(int id);

        Task<PlacementOfficerResponse?> GetByUserIdAsync(string userId);

        Task<PlacementOfficerResponse> RegisterAsync(
            CreatePlacementOfficerRequest request);

        Task<bool> ApproveAsync(int id, string? approvedByUserId);

        Task<bool> RejectAsync(int id);

        Task<bool> SuspendAsync(int id);
    }
}
