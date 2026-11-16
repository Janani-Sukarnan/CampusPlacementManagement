using CampusPlacementManagement.DTOs.ApplicationStatusHistories;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services
{
    public class ApplicationStatusHistoryService :
        IApplicationStatusHistoryService
    {
        private readonly IApplicationStatusHistoryRepository _repository;

        public ApplicationStatusHistoryService(
            IApplicationStatusHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ApplicationStatusHistoryResponse>>
            GetAllAsync()
        {
            var histories = await _repository.GetAllAsync();

            return histories.Select(MapToResponse);
        }

        public async Task<IEnumerable<ApplicationStatusHistoryResponse>>
            GetByJobApplicationIdAsync(int jobApplicationId)
        {
            var histories =
                await _repository.GetByJobApplicationIdAsync(jobApplicationId);

            return histories.Select(MapToResponse);
        }

        private static ApplicationStatusHistoryResponse MapToResponse(
            Models.ApplicationStatusHistory history)
        {
            return new ApplicationStatusHistoryResponse
            {
                ApplicationStatusHistoryId =
                    history.ApplicationStatusHistoryId,

                JobApplicationId = history.JobApplicationId,

                PreviousStatus = history.PreviousStatus.ToString(),

                NewStatus = history.NewStatus.ToString(),

                ChangedAt = history.ChangedAt,

                ChangedByUserId = history.ChangedByUserId,

                ChangedByName = history.ChangedByUser?.FullName
                    ?? string.Empty,

                Remarks = history.Remarks
            };
        }
    }
}