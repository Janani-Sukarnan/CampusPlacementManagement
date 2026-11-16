using CampusPlacementManagement.DTOs.Interviews;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IInterviewService
    {
        Task<IEnumerable<InterviewResponse>> GetAllAsync();

        Task<IEnumerable<InterviewResponse>> GetMyInterviewsAsync(
            string userId);

        Task<IEnumerable<InterviewResponse>> GetCompanyInterviewsAsync(
            string userId);

        Task<InterviewResponse?> GetByIdAsync(int id);

        Task<InterviewResponse> ScheduleAsync(
            CreateInterviewRequest request,
            string userId);

        Task<InterviewResponse> UpdateStatusAsync(
            int id,
            UpdateInterviewStatusRequest request,
            string userId,
            bool isSuperAdmin);
    }
}