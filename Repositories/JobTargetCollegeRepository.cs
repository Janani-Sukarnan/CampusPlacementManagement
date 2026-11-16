
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class JobTargetCollegeRepository : IJobTargetCollegeRepository
    {
        private readonly ApplicationDbContext _context;

        public JobTargetCollegeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobTargetCollege>> GetAllAsync()
        {
            return await _context.JobTargetColleges
                .Include(j => j.JobOpening)
                .Include(j => j.College)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<JobTargetCollege?> GetByIdAsync(int id)
        {
            return await _context.JobTargetColleges
                .Include(j => j.JobOpening)
                .Include(j => j.College)
                .FirstOrDefaultAsync(j => j.JobTargetCollegeId == id);
        }

        public async Task<IEnumerable<JobTargetCollege>> GetByCompanyIdAsync(
            int companyId)
        {
            return await _context.JobTargetColleges
                .Include(j => j.JobOpening)
                .Include(j => j.College)
                .Where(j => j.JobOpening.CompanyId == companyId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(
            int jobOpeningId, int collegeId)
        {
            return await _context.JobTargetColleges
                .AnyAsync(j =>
                    j.JobOpeningId == jobOpeningId &&
                    j.CollegeId == collegeId);
        }

        public async Task AddAsync(JobTargetCollege jobTargetCollege)
        {
            await _context.JobTargetColleges.AddAsync(jobTargetCollege);
            await _context.SaveChangesAsync();
        }
    }
}
