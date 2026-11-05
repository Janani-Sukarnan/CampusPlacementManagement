
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class CollegeRepository : ICollegeRepository
    {
        private readonly ApplicationDbContext _context;

        public CollegeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<College>> GetAllCollegesAsync()
        {
            return await _context.Colleges
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<College?> GetCollegeByIdAsync(int collegeId)
        {
            return await _context.Colleges
                .FirstOrDefaultAsync(c => c.CollegeId == collegeId);
        }

        public async Task<College?> GetCollegeByCodeAsync(string collegeCode)
        {
            return await _context.Colleges
                .FirstOrDefaultAsync(c => c.CollegeCode == collegeCode);
        }

        public async Task<College> AddCollegeAsync(College college)
        {
            await _context.Colleges.AddAsync(college);
            await _context.SaveChangesAsync();

            return college;
        }

        public async Task<College> UpdateCollegeAsync(College college)
        {
            _context.Colleges.Update(college);
            await _context.SaveChangesAsync();

            return college;
        }

        public async Task<bool> DeleteCollegeAsync(int collegeId)
        {
            var college = await GetCollegeByIdAsync(collegeId);

            if (college == null)
            {
                return false;
            }

            _context.Colleges.Remove(college);
            await _context.SaveChangesAsync();

            return true;
        }
    
    
public async Task<IEnumerable<College>> SearchCollegesAsync(string searchTerm)
        {
            return await _context.Colleges
                .AsNoTracking()
                .Where(c =>
                    c.CollegeName.Contains(searchTerm) ||
                    c.CollegeCode.Contains(searchTerm) ||
                    (c.UniversityName != null &&
                     c.UniversityName.Contains(searchTerm)) ||
                    c.OfficialEmail.Contains(searchTerm))
                .ToListAsync();
        }

    }
}