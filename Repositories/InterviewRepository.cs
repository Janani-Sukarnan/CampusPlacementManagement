using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Interview>> GetAllAsync()
        {
            return await _context.Interviews
                .Include(i => i.JobApplication)
                    .ThenInclude(a => a!.Student)
                        .ThenInclude(s => s!.User)
                .Include(i => i.JobApplication)
                    .ThenInclude(a => a!.JobOpening)
                        .ThenInclude(j => j!.Company)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Interview>> GetByStudentIdAsync(
            int studentId)
        {
            return await _context.Interviews
                .Include(i => i.JobApplication)
                    .ThenInclude(a => a!.Student)
                        .ThenInclude(s => s!.User)
                .Include(i => i.JobApplication)
                    .ThenInclude(a => a!.JobOpening)
                        .ThenInclude(j => j!.Company)
                .Where(i => i.JobApplication!.StudentId == studentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Interview>> GetByCompanyIdAsync(
            int companyId)
        {
            return await _context.Interviews
                .Include(i => i.JobApplication)
                    .ThenInclude(a => a!.Student)
                        .ThenInclude(s => s!.User)
                .Include(i => i.JobApplication)
                    .ThenInclude(a => a!.JobOpening)
                        .ThenInclude(j => j!.Company)
                .Where(i => i.JobApplication!.JobOpening!.CompanyId == companyId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Interview?> GetByIdAsync(int id)
        {
            return await _context.Interviews
                .Include(i => i.JobApplication)
                    .ThenInclude(a => a!.Student)
                        .ThenInclude(s => s!.User)
                .Include(i => i.JobApplication)
                    .ThenInclude(a => a!.JobOpening)
                        .ThenInclude(j => j!.Company)
                .FirstOrDefaultAsync(i => i.InterviewId == id);
        }

        public async Task AddAsync(Interview interview)
        {
            await _context.Interviews.AddAsync(interview);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Interview interview)
        {
            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();
        }
    }
}