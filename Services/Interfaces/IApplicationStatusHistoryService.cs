using CampusPlacementManagement.DTOs.ApplicationStatusHistories;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IApplicationStatusHistoryService
    {
        Task<IEnumerable<ApplicationStatusHistoryResponse>> GetAllAsync();

        Task<IEnumerable<ApplicationStatusHistoryResponse>>
            GetByJobApplicationIdAsync(int jobApplicationId);
    }
}