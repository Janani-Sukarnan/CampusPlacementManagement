
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class PlacementOfficerRepository : IPlacementOfficerRepository
    {
        private readonly ApplicationDbContext _context;

        public PlacementOfficerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlacementOfficer>> GetAllAsync()
        {
            return await _context.Set<PlacementOfficer>()
                .Include(p => p.User)
                .Include(p => p.College)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlacementOfficer>> GetPendingAsync()
        {
            return await _context.Set<PlacementOfficer>()
                .Include(p => p.User)
                .Include(p => p.College)
                .Where(p => p.Status == AccountStatus.Pending)
                .ToListAsync();
        }

        public async Task<PlacementOfficer?> GetByIdAsync(int id)
        {
            return await _context.Set<PlacementOfficer>()
                .Include(p => p.User)
                .Include(p => p.College)
                .Include(p => p.Approver)
                .FirstOrDefaultAsync(p => p.PlacementOfficerId == id);
        }

        public async Task<PlacementOfficer?> GetByUserIdAsync(string userId)
        {
            return await _context.Set<PlacementOfficer>()
                .Include(p => p.User)
                .Include(p => p.College)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<PlacementOfficer> AddAsync(PlacementOfficer officer)
        {
            await _context.Set<PlacementOfficer>().AddAsync(officer);
            await _context.SaveChangesAsync();

            return officer;
        }

        public async Task<PlacementOfficer> UpdateAsync(PlacementOfficer officer)
        {
            _context.Set<PlacementOfficer>().Update(officer);
            await _context.SaveChangesAsync();

            return officer;
        }
    }
}
