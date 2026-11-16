
using System.Security.Claims;
using CampusPlacementManagement.DTOs.JobTargetColleges;
using CampusPlacementManagement.Helpers;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/job-target-colleges")]
    [ApiController]
    [Authorize]
    public class JobTargetCollegesController : ControllerBase
    {
        private readonly IJobTargetCollegeService _service;

        public JobTargetCollegesController(
            IJobTargetCollegeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.SuperAdmin)]
        public async Task<IActionResult> GetAll()
        {
            var targets = await _service.GetAllAsync();
            return Ok(targets);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = RoleConstants.SuperAdmin)]
        public async Task<IActionResult> GetById(int id)
        {
            var target = await _service.GetByIdAsync(id);

            if (target == null)
                return NotFound(new { message = "Target assignment not found." });

            return Ok(target);
        }

        [HttpGet("my-company")]
        [Authorize(Roles = RoleConstants.CompanyHR)]
        public async Task<IActionResult> GetMyCompanyTargets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                var targets = await _service.GetMyCompanyTargetsAsync(userId);
                return Ok(targets);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.CompanyHR)]
        public async Task<IActionResult> Create(
            CreateJobTargetCollegeRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                var target = await _service.CreateAsync(request, userId);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = target.JobTargetCollegeId },
                    target);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}
