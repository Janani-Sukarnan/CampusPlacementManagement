
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class JobOpeningRepository : IJobOpeningRepository
    {
        private readonly ApplicationDbContext _context;

        public JobOpeningRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobOpening>> GetAllAsync()
        {
            return await _context.Set<JobOpening>()
                .Include(j => j.Company)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetByCompanyIdAsync(
            int companyId)
        {
            return await _context.Set<JobOpening>()
                .Include(j => j.Company)
                .Where(j => j.CompanyId == companyId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<JobOpening>> GetOpenAsync()
        {
            return await _context.Set<JobOpening>()
                .Include(j => j.Company)
                .Where(j => j.Status == JobOpeningStatus.Open &&
                            j.ApplicationDeadline >= DateTime.UtcNow)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<JobOpening?> GetByIdAsync(int id)
        {
            return await _context.Set<JobOpening>()
                .Include(j => j.Company)
                .FirstOrDefaultAsync(j => j.JobOpeningId == id);
        }

        public async Task<JobOpening> AddAsync(JobOpening jobOpening)
        {
            await _context.Set<JobOpening>().AddAsync(jobOpening);
            await _context.SaveChangesAsync();

            return jobOpening;
        }

        public async Task<JobOpening> UpdateAsync(JobOpening jobOpening)
        {
            _context.Set<JobOpening>().Update(jobOpening);
            await _context.SaveChangesAsync();

            return jobOpening;
        }
    }
}
