using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<CompanyResponse> RegisterCompanyAsync(
            CreateCompanyRequest request)
        {
            var existingCompany = await _companyRepository
                .GetCompanyByEmailAsync(request.CompanyEmail);

            if (existingCompany != null)
            {
                throw new InvalidOperationException(
                    "A company with this email already exists.");
            }

            var company = new Company
            {
                CompanyName = request.CompanyName,
                CompanyEmail = request.CompanyEmail,
                Website = request.Website,
                Industry = request.Industry,
                Address = request.Address,
                Description = request.Description,
                Status = CompanyStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var createdCompany = await _companyRepository
                .AddCompanyAsync(company);

            return MapToResponse(createdCompany);
        }

        public async Task<IEnumerable<CompanyResponse>> GetAllCompaniesAsync()
        {
            var companies = await _companyRepository.GetAllCompaniesAsync();

            return companies.Select(MapToResponse);
        }

        public async Task<IEnumerable<CompanyResponse>> GetPendingCompaniesAsync()
        {
            var companies = await _companyRepository.GetPendingCompaniesAsync();

            return companies.Select(MapToResponse);
        }

        public async Task<CompanyResponse?> GetCompanyByIdAsync(int companyId)
        {
            var company = await _companyRepository
                .GetCompanyByIdAsync(companyId);

            return company == null ? null : MapToResponse(company);
        }

        public async Task<CompanyResponse> UpdateCompanyAsync(
            int companyId,
            UpdateCompanyRequest request)
        {
            var company = await _companyRepository
                .GetCompanyByIdAsync(companyId);

            if (company == null)
            {
                throw new InvalidOperationException("Company not found.");
            }

            var existingCompany = await _companyRepository
                .GetCompanyByEmailAsync(request.CompanyEmail);

            if (existingCompany != null &&
                existingCompany.CompanyId != companyId)
            {
                throw new InvalidOperationException(
                    "Another company is already registered with this email.");
            }

            company.CompanyName = request.CompanyName;
            company.CompanyEmail = request.CompanyEmail;
            company.Website = request.Website;
            company.Industry = request.Industry;
            company.Address = request.Address;
            company.Description = request.Description;

            var updatedCompany = await _companyRepository
                .UpdateCompanyAsync(company);

            return MapToResponse(updatedCompany);
        }

        public async Task<bool> ApproveCompanyAsync(int companyId)
        {
            var company = await _companyRepository
                .GetCompanyByIdAsync(companyId);

            if (company == null ||
                company.Status != CompanyStatus.Pending)
            {
                return false;
            }

            company.Status = CompanyStatus.Approved;

            await _companyRepository.UpdateCompanyAsync(company);

            return true;
        }

        public async Task<bool> RejectCompanyAsync(int companyId)
        {
            var company = await _companyRepository
                .GetCompanyByIdAsync(companyId);

            if (company == null ||
                company.Status != CompanyStatus.Pending)
            {
                return false;
            }

            company.Status = CompanyStatus.Rejected;

            await _companyRepository.UpdateCompanyAsync(company);

            return true;
        }

        public async Task<bool> SuspendCompanyAsync(int companyId)
        {
            var company = await _companyRepository
                .GetCompanyByIdAsync(companyId);

            if (company == null ||
                company.Status != CompanyStatus.Approved)
            {
                return false;
            }

            company.Status = CompanyStatus.Suspended;

            await _companyRepository.UpdateCompanyAsync(company);

            return true;
        }

        private static CompanyResponse MapToResponse(Company company)
        {
            return new CompanyResponse
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                CompanyEmail = company.CompanyEmail,
                Website = company.Website,
                Industry = company.Industry,
                Address = company.Address,
                Description = company.Description,
                Status = company.Status,
                CreatedAt = company.CreatedAt
            };
        }
    }
}