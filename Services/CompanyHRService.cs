using CampusPlacementManagement.DTOs.CompanyHRs;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Helpers;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Services
{
    public class CompanyHRService : ICompanyHRService
    {
        private readonly ICompanyHRRepository _companyHRRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CompanyHRService(
            ICompanyHRRepository companyHRRepository,
            ICompanyRepository companyRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _companyHRRepository = companyHRRepository;
            _companyRepository = companyRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET ALL COMPANY HRs
        public async Task<IEnumerable<CompanyHRResponse>> GetAllAsync()
        {
            var companyHRs = await _companyHRRepository.GetAllAsync();

            return companyHRs.Select(MapToResponse);
        }

        // GET COMPANY HR BY ID
        public async Task<CompanyHRResponse?> GetByIdAsync(int id)
        {
            var companyHR = await _companyHRRepository.GetByIdAsync(id);

            return companyHR == null ? null : MapToResponse(companyHR);
        }

        // GET COMPANY HR BY USER ID
        public async Task<CompanyHRResponse?> GetByUserIdAsync(string userId)
        {
            var companyHR = await _companyHRRepository.GetByUserIdAsync(userId);

            if (companyHR == null)
                return null;

            // Load the Identity user if the repository did not include it.
            companyHR.User = await _userManager.FindByIdAsync(companyHR.UserId)
                ?? throw new InvalidOperationException("Company HR user not found.");

            return MapToResponse(companyHR);
        }

        // GET PENDING COMPANY HRs
        public async Task<IEnumerable<CompanyHRResponse>> GetPendingAsync()
        {
            var companyHRs = await _companyHRRepository.GetPendingAsync();

            return companyHRs
                .Where(x => x.User.AccountStatus == AccountStatus.Pending)
                .Select(MapToResponse);
        }

        // REGISTER COMPANY HR
        public async Task<CompanyHRResponse> RegisterAsync(
            CreateCompanyHRRequest request)
        {
            var company = await _companyRepository
                .GetCompanyByIdAsync(request.CompanyId);

            if (company == null)
                throw new InvalidOperationException("Company not found.");

            if (company.Status != CompanyStatus.Approved)
                throw new InvalidOperationException(
                    "Only approved companies can register HR representatives.");

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
                throw new InvalidOperationException(
                    "An account with this email already exists.");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                AccountStatus = AccountStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(
                user, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    createResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }

            try
            {
                // Create the CompanyHR role if it does not exist.
                if (!await _roleManager.RoleExistsAsync(RoleConstants.CompanyHR))
                {
                    var roleResult = await _roleManager.CreateAsync(
                        new IdentityRole(RoleConstants.CompanyHR));

                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(
                            "; ",
                            roleResult.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(errors);
                    }
                }

                // Assign the CompanyHR role to the new user.
                var roleAssignment = await _userManager.AddToRoleAsync(
                    user, RoleConstants.CompanyHR);

                if (!roleAssignment.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleAssignment.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(errors);
                }

                var companyHR = new CompanyHR
                {
                    UserId = user.Id,
                    CompanyId = request.CompanyId,
                    Designation = request.Designation
                };

                await _companyHRRepository.AddAsync(companyHR);

                var savedCompanyHR = await _companyHRRepository
                    .GetByIdAsync(companyHR.CompanyHRId);

                if (savedCompanyHR == null)
                    throw new InvalidOperationException(
                        "Unable to retrieve the registered Company HR.");

                return MapToResponse(savedCompanyHR);
            }
            catch
            {
                // Avoid leaving an Identity account without a CompanyHR record.
                await _userManager.DeleteAsync(user);
                throw;
            }
        }

        // APPROVE COMPANY HR
        public async Task<bool> ApproveAsync(int id, string approvedBy)
        {
            var companyHR = await _companyHRRepository.GetByIdAsync(id);

            if (companyHR == null)
                return false;

            if (companyHR.User.AccountStatus != AccountStatus.Pending)
                return false;

            companyHR.User.AccountStatus = AccountStatus.Approved;
            companyHR.User.UpdatedAt = DateTime.UtcNow;

            companyHR.ApprovedBy = approvedBy;
            companyHR.ApprovedAt = DateTime.UtcNow;

            await _companyHRRepository.UpdateAsync(companyHR);

            return true;
        }

        // REJECT COMPANY HR
        public async Task<bool> RejectAsync(int id)
        {
            var companyHR = await _companyHRRepository.GetByIdAsync(id);

            if (companyHR == null)
                return false;

            if (companyHR.User.AccountStatus != AccountStatus.Pending)
                return false;

            companyHR.User.AccountStatus = AccountStatus.Rejected;
            companyHR.User.UpdatedAt = DateTime.UtcNow;

            await _companyHRRepository.UpdateAsync(companyHR);

            return true;
        }

        // MAP ENTITY TO RESPONSE DTO
        private static CompanyHRResponse MapToResponse(CompanyHR companyHR)
        {
            return new CompanyHRResponse
            {
                CompanyHRId = companyHR.CompanyHRId,
                UserId = companyHR.UserId,
                FullName = companyHR.User.FullName,
                Email = companyHR.User.Email ?? string.Empty,
                CompanyId = companyHR.CompanyId,
                CompanyName = companyHR.Company.CompanyName,
                Designation = companyHR.Designation,
                ApprovedBy = companyHR.ApprovedBy,
                ApprovedAt = companyHR.ApprovedAt
            };
        }
    }
}