
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class RecruitmentDriveRepository : IRecruitmentDriveRepository
    {
        private readonly ApplicationDbContext _context;

        public RecruitmentDriveRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecruitmentDrive>> GetAllAsync()
        {
            return await _context.RecruitmentDrives
                .Include(r => r.Company)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<RecruitmentDrive>> GetByCompanyIdAsync(
            int companyId)
        {
            return await _context.RecruitmentDrives
                .Include(r => r.Company)
                .Where(r => r.CompanyId == companyId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<RecruitmentDrive?> GetByIdAsync(int id)
        {
            return await _context.RecruitmentDrives
                .Include(r => r.Company)
                .FirstOrDefaultAsync(r => r.RecruitmentDriveId == id);
        }

        public async Task<RecruitmentDrive> AddAsync(
            RecruitmentDrive recruitmentDrive)
        {
            await _context.RecruitmentDrives.AddAsync(recruitmentDrive);
            await _context.SaveChangesAsync();

            return recruitmentDrive;
        }

        public async Task<RecruitmentDrive> UpdateAsync(
            RecruitmentDrive recruitmentDrive)
        {
            _context.RecruitmentDrives.Update(recruitmentDrive);
            await _context.SaveChangesAsync();

            return recruitmentDrive;
        }
    }
}
