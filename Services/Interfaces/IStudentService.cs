using CampusPlacementManagement.DTOs.Students;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentResponse>> GetAllAsync();

        Task<IEnumerable<StudentResponse>> GetPendingAsync();

        Task<StudentResponse?> GetByIdAsync(int id);

        Task<StudentResponse?> GetByUserIdAsync(string userId);

        Task<StudentResponse> RegisterAsync(
            CreateStudentRequest request);

        Task<StudentResponse?> UpdateAsync(
            int id,
            UpdateStudentRequest request);

        Task<bool> ApproveAsync(int id);

        Task<bool> RejectAsync(int id);
    }
}