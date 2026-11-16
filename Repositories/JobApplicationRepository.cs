
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobApplication>> GetAllAsync()
        {
            return await _context.JobApplications
                .Include(j => j.Student)
                    .ThenInclude(s => s!.User)
                .Include(j => j.JobOpening)
                    .ThenInclude(o => o!.Company)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetByStudentIdAsync(
            int studentId)
        {
            return await _context.JobApplications
                .Include(j => j.Student)
                    .ThenInclude(s => s!.User)
                .Include(j => j.JobOpening)
                    .ThenInclude(o => o!.Company)
                .Where(j => j.StudentId == studentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<JobApplication>> GetByCompanyIdAsync(
            int companyId)
        {
            return await _context.JobApplications
                .Include(j => j.Student)
                    .ThenInclude(s => s!.User)
                .Include(j => j.JobOpening)
                    .ThenInclude(o => o!.Company)
                .Where(j => j.JobOpening!.CompanyId == companyId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<JobApplication?> GetByIdAsync(int id)
        {
            return await _context.JobApplications
                .Include(j => j.Student)
                    .ThenInclude(s => s!.User)
                .Include(j => j.JobOpening)
                    .ThenInclude(o => o!.Company)
                .FirstOrDefaultAsync(j => j.JobApplicationId == id);
        }

        public async Task<bool> ExistsAsync(
            int studentId, int jobOpeningId)
        {
            return await _context.JobApplications.AnyAsync(j =>
                j.StudentId == studentId &&
                j.JobOpeningId == jobOpeningId);
        }

        public async Task AddAsync(JobApplication jobApplication)
        {
            await _context.JobApplications.AddAsync(jobApplication);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobApplication jobApplication)
        {
            _context.JobApplications.Update(jobApplication);
            await _context.SaveChangesAsync();
        }
    }
}
