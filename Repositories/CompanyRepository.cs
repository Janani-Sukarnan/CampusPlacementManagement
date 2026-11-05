using CampusPlacementManagement.Data;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _context.Set<Company>()
                .ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetPendingCompaniesAsync()
        {
            return await _context.Set<Company>()
                .Where(c => c.Status == CompanyStatus.Pending)
                .ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int companyId)
        {
            return await _context.Set<Company>()
                .FirstOrDefaultAsync(c => c.CompanyId == companyId);
        }

        public async Task<Company?> GetCompanyByEmailAsync(string companyEmail)
        {
            return await _context.Set<Company>()
                .FirstOrDefaultAsync(c => c.CompanyEmail == companyEmail);
        }

        public async Task<Company> AddCompanyAsync(Company company)
        {
            await _context.Set<Company>().AddAsync(company);
            await _context.SaveChangesAsync();

            return company;
        }

        public async Task<Company> UpdateCompanyAsync(Company company)
        {
            _context.Set<Company>().Update(company);
            await _context.SaveChangesAsync();

            return company;
        }
    }
}