
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IRecruitmentDriveRepository
    {
        Task<IEnumerable<RecruitmentDrive>> GetAllAsync();

        Task<IEnumerable<RecruitmentDrive>> GetByCompanyIdAsync(int companyId);

        Task<RecruitmentDrive?> GetByIdAsync(int id);

        Task<RecruitmentDrive> AddAsync(RecruitmentDrive recruitmentDrive);

        Task<RecruitmentDrive> UpdateAsync(RecruitmentDrive recruitmentDrive);
    }
}
