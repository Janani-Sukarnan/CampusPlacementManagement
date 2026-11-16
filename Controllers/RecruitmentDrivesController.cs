
using System.Security.Claims;
using CampusPlacementManagement.DTOs.RecruitmentDrives;
using CampusPlacementManagement.Helpers;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/recruitment-drives")]
    public class RecruitmentDrivesController : ControllerBase
    {
        private readonly IRecruitmentDriveService _recruitmentDriveService;

        public RecruitmentDrivesController(
            IRecruitmentDriveService recruitmentDriveService)
        {
            _recruitmentDriveService = recruitmentDriveService;
        }

        // SUPERADMIN / STUDENT: Get all recruitment drives
        [Authorize(Roles = RoleConstants.SuperAdmin + "," + RoleConstants.Student)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var drives = await _recruitmentDriveService.GetAllAsync();
            return Ok(drives);
        }

        // COMPANY HR: Get own company's recruitment drives
        [Authorize(Roles = RoleConstants.CompanyHR)]
        [HttpGet("my-company")]
        public async Task<IActionResult> GetMyCompanyDrives()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var drives = await _recruitmentDriveService
                    .GetByCompanyHRAsync(userId);

                return Ok(drives);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // SUPERADMIN / STUDENT: Get recruitment drive by ID
        [Authorize(Roles = RoleConstants.SuperAdmin + "," + RoleConstants.Student)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var drive = await _recruitmentDriveService.GetByIdAsync(id);

            if (drive == null)
                return NotFound(new { message = "Recruitment drive not found." });

            return Ok(drive);
        }

        // COMPANY HR: Create recruitment drive
        [Authorize(Roles = RoleConstants.CompanyHR)]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateRecruitmentDriveRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var drive = await _recruitmentDriveService.CreateAsync(
                    request, userId);

                return Ok(drive);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // COMPANY HR / SUPERADMIN: Update recruitment drive
        [Authorize(Roles = RoleConstants.CompanyHR + "," + RoleConstants.SuperAdmin)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateRecruitmentDriveRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            bool isSuperAdmin = User.IsInRole(RoleConstants.SuperAdmin);

            try
            {
                var drive = await _recruitmentDriveService.UpdateAsync(
                    id, request, userId, isSuperAdmin);

                if (drive == null)
                    return NotFound(new { message = "Recruitment drive not found." });

                return Ok(drive);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
