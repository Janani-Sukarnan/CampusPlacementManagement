
using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Services
{
    public class PlacementOfficerService : IPlacementOfficerService
    {
        private readonly IPlacementOfficerRepository _repository;
        private readonly ICollegeRepository _collegeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PlacementOfficerService(
            IPlacementOfficerRepository repository,
            ICollegeRepository collegeRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _collegeRepository = collegeRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<PlacementOfficerResponse>> GetAllAsync()
        {
            var officers = await _repository.GetAllAsync();
            return officers.Select(MapToResponse);
        }

        public async Task<IEnumerable<PlacementOfficerResponse>> GetPendingAsync()
        {
            var officers = await _repository.GetPendingAsync();
            return officers.Select(MapToResponse);
        }

        public async Task<PlacementOfficerResponse?> GetByIdAsync(int id)
        {
            var officer = await _repository.GetByIdAsync(id);

            return officer == null ? null : MapToResponse(officer);
        }

        public async Task<PlacementOfficerResponse?> GetByUserIdAsync(string userId)
        {
            var officer = await _repository.GetByUserIdAsync(userId);

            return officer == null ? null : MapToResponse(officer);
        }

        public async Task<PlacementOfficerResponse> RegisterAsync(
            CreatePlacementOfficerRequest request)
        {
            var college = await _collegeRepository.GetCollegeByIdAsync(
                request.CollegeId);

            if (college == null || college.Status != CollegeStatus.Approved)
            {
                throw new InvalidOperationException(
                    "Placement officers can register only with an approved college.");
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException(
                    "An account with this email already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                FullName = request.FullName,
                AccountStatus = AccountStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            var officer = new PlacementOfficer
            {
                UserId = user.Id,
                CollegeId = request.CollegeId,
                Designation = request.Designation,
                Status = AccountStatus.Pending
            };

            try
            {
                await _repository.AddAsync(officer);
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                throw;
            }

            var createdOfficer = await _repository.GetByIdAsync(
                officer.PlacementOfficerId);

            return MapToResponse(createdOfficer!);
        }

        public async Task<bool> ApproveAsync(
            int id, string? approvedByUserId)
        {
            var officer = await _repository.GetByIdAsync(id);

            if (officer == null)
                return false;

            var user = await _userManager.FindByIdAsync(officer.UserId);

            if (user == null)
                throw new InvalidOperationException("Officer user account not found.");

            const string roleName = "PlacementOfficer";

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await _roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, roleName);

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            officer.Status = AccountStatus.Approved;
            officer.ApprovedBy = approvedByUserId;
            officer.ApprovedAt = DateTime.UtcNow;

            user.AccountStatus = AccountStatus.Approved;
            user.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(officer);
            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<bool> RejectAsync(int id)
        {
            var officer = await _repository.GetByIdAsync(id);

            if (officer == null)
                return false;

            officer.Status = AccountStatus.Rejected;

            var user = await _userManager.FindByIdAsync(officer.UserId);

            if (user != null)
            {
                user.AccountStatus = AccountStatus.Rejected;
                user.UpdatedAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }

            await _repository.UpdateAsync(officer);

            return true;
        }

        public async Task<bool> SuspendAsync(int id)
        {
            var officer = await _repository.GetByIdAsync(id);

            if (officer == null)
                return false;

            officer.Status = AccountStatus.Suspended;

            var user = await _userManager.FindByIdAsync(officer.UserId);

            if (user != null)
            {
                user.AccountStatus = AccountStatus.Suspended;
                user.UpdatedAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }

            await _repository.UpdateAsync(officer);

            return true;
        }

        private static PlacementOfficerResponse MapToResponse(
            PlacementOfficer officer)
        {
            return new PlacementOfficerResponse
            {
                PlacementOfficerId = officer.PlacementOfficerId,
                UserId = officer.UserId,
                FullName = officer.User?.FullName ?? string.Empty,
                Email = officer.User?.Email ?? string.Empty,
                PhoneNumber = officer.User?.PhoneNumber ?? string.Empty,
                CollegeId = officer.CollegeId,
                CollegeName = officer.College?.CollegeName ?? string.Empty,
                Designation = officer.Designation,
                Status = officer.Status,
                ApprovedBy = officer.ApprovedBy,
                ApprovedAt = officer.ApprovedAt
            };
        }
    }
}
