using CampusPlacementManagement.Data;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.College)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.College)
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }

        public async Task<Student?> GetByUserIdAsync(string userId)
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.College)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByRegistrationNumberAsync(
            string registrationNumber)
        {
            return await _context.Students
                .AnyAsync(s =>
                    s.RegistrationNumber == registrationNumber);
        }
        public async Task<IEnumerable<Student>> GetPendingAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.College)
                .Where(s => s.User != null &&
                            s.User.AccountStatus == AccountStatus.Pending)
                .ToListAsync();
        }
    }
}