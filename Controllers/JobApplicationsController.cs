
using System.Security.Claims;
using CampusPlacementManagement.DTOs.JobApplications;
using CampusPlacementManagement.Helpers;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/job-applications")]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IJobApplicationService _service;

        public JobApplicationsController(IJobApplicationService service)
        {
            _service = service;
        }

        // SUPERADMIN: Get all applications
        [Authorize(Roles = RoleConstants.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var applications = await _service.GetAllAsync();
            return Ok(applications);
        }

        // STUDENT: Get own applications
        [Authorize(Roles = RoleConstants.Student)]
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var applications =
                    await _service.GetMyApplicationsAsync(userId);

                return Ok(applications);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // COMPANY HR: Get applications for own company
        [Authorize(Roles = RoleConstants.CompanyHR)]
        [HttpGet("company")]
        public async Task<IActionResult> GetCompanyApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var applications =
                    await _service.GetCompanyApplicationsAsync(userId);

                return Ok(applications);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // SUPERADMIN: Get application by ID
        [Authorize(Roles = RoleConstants.SuperAdmin)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var application = await _service.GetByIdAsync(id);

            if (application == null)
                return NotFound(new { message = "Job application not found." });

            return Ok(application);
        }

        // STUDENT: Apply for a job
        [Authorize(Roles = RoleConstants.Student)]
        [HttpPost]
        public async Task<IActionResult> Apply(
            CreateJobApplicationRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var application = await _service.ApplyAsync(request, userId);
                return Ok(application);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // COMPANY HR / SUPERADMIN: Update application status
        [Authorize(Roles = RoleConstants.CompanyHR + "," + RoleConstants.SuperAdmin)]
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            UpdateJobApplicationStatusRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            bool isSuperAdmin = User.IsInRole(RoleConstants.SuperAdmin);

            try
            {
                var application = await _service.UpdateStatusAsync(
                    id, request, userId, isSuperAdmin);

                return Ok(application);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
