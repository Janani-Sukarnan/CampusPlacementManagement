
using System.Security.Claims;
using CampusPlacementManagement.DTOs.JobOpenings;
using CampusPlacementManagement.Helpers;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/job-openings")]
    public class JobOpeningsController : ControllerBase
    {
        private readonly IJobOpeningService _jobOpeningService;

        public JobOpeningsController(IJobOpeningService jobOpeningService)
        {
            _jobOpeningService = jobOpeningService;
        }

        // SUPERADMIN: Get all job openings
        [Authorize(Roles = RoleConstants.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await _jobOpeningService.GetAllAsync();
            return Ok(jobs);
        }

        // STUDENT: Get open job opportunities
        [Authorize(Roles = RoleConstants.Student)]
        [HttpGet("open")]
        public async Task<IActionResult> GetOpen()
        {
            var jobs = await _jobOpeningService.GetOpenAsync();
            return Ok(jobs);
        }

        // COMPANY HR: Get own company's job openings
        [Authorize(Roles = RoleConstants.CompanyHR)]
        [HttpGet("my-company")]
        public async Task<IActionResult> GetMyCompanyJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var jobs = await _jobOpeningService
                    .GetByCompanyHRAsync(userId);

                return Ok(jobs);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // SUPERADMIN / STUDENT: Get job by ID
        [Authorize(Roles = RoleConstants.SuperAdmin + "," + RoleConstants.Student)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _jobOpeningService.GetByIdAsync(id);

            if (job == null)
                return NotFound(new { message = "Job opening not found." });

            return Ok(job);
        }

        // COMPANY HR: Create job opening
        [Authorize(Roles = RoleConstants.CompanyHR)]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateJobOpeningRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var job = await _jobOpeningService.CreateAsync(
                    request, userId);

                return Ok(job);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // COMPANY HR / SUPERADMIN: Update job opening
        [Authorize(Roles = RoleConstants.CompanyHR + "," + RoleConstants.SuperAdmin)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id, UpdateJobOpeningRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            bool isSuperAdmin =
                User.IsInRole(RoleConstants.SuperAdmin);

            try
            {
                var job = await _jobOpeningService.UpdateAsync(
                    id, request, userId, isSuperAdmin);

                if (job == null)
                    return NotFound(new { message = "Job opening not found." });

                return Ok(job);
            }
            catch (UnauthorizedAccessException ex)
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
