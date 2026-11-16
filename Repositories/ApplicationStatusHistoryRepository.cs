using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class ApplicationStatusHistoryRepository :
        IApplicationStatusHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationStatusHistoryRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationStatusHistory>> GetAllAsync()
        {
            return await _context.ApplicationStatusHistories
                .Include(h => h.ChangedByUser)
                .Include(h => h.JobApplication)
                .OrderByDescending(h => h.ChangedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationStatusHistory>>
            GetByJobApplicationIdAsync(int jobApplicationId)
        {
            return await _context.ApplicationStatusHistories
                .Include(h => h.ChangedByUser)
                .Where(h => h.JobApplicationId == jobApplicationId)
                .OrderByDescending(h => h.ChangedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(ApplicationStatusHistory history)
        {
            await _context.ApplicationStatusHistories.AddAsync(history);
            await _context.SaveChangesAsync();
        }
    }
}