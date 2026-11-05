using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class CompanyHRRepository : ICompanyHRRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyHRRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyHR>> GetAllAsync()
        {
            return await _context.CompanyHRs
                .Include(x => x.Company)
                .Include(x => x.User)
                .ToListAsync();
        }

        public async Task<CompanyHR?> GetByIdAsync(int id)
        {
            return await _context.CompanyHRs
                .Include(x => x.Company)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.CompanyHRId == id);
        }

        public async Task<CompanyHR?> GetByUserIdAsync(string userId)
        {
            return await _context.CompanyHRs
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<CompanyHR>> GetPendingAsync()
        {
            return await _context.CompanyHRs
                .Include(x => x.Company)
                .Include(x => x.User)
                .Where(x => x.ApprovedAt == null)
                .ToListAsync();
        }

        public async Task AddAsync(CompanyHR companyHR)
        {
            await _context.CompanyHRs.AddAsync(companyHR);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CompanyHR companyHR)
        {
            _context.CompanyHRs.Update(companyHR);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.CompanyHRs
                .AnyAsync(x => x.CompanyHRId == id);
        }
    }
}