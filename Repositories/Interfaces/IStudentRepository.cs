using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();

        Task<Student?> GetByIdAsync(int id);

        Task<Student?> GetByUserIdAsync(string userId);

        Task AddAsync(Student student);

        Task UpdateAsync(Student student);

        Task<bool> ExistsByRegistrationNumberAsync(
            string registrationNumber);
        Task<IEnumerable<Student>> GetPendingAsync();
    }
}