using System.Security.Claims;
using CampusPlacementManagement.DTOs.Interviews;
using CampusPlacementManagement.Helpers;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/interviews")]
    public class InterviewsController : ControllerBase
    {
        private readonly IInterviewService _service;

        public InterviewsController(IInterviewService service)
        {
            _service = service;
        }

        [Authorize(Roles = RoleConstants.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [Authorize(Roles = RoleConstants.Student)]
        [HttpGet("my-interviews")]
        public async Task<IActionResult> GetMyInterviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                return Ok(await _service.GetMyInterviewsAsync(userId));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = RoleConstants.CompanyHR)]
        [HttpGet("company")]
        public async Task<IActionResult> GetCompanyInterviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                return Ok(await _service.GetCompanyInterviewsAsync(userId));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = RoleConstants.SuperAdmin)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var interview = await _service.GetByIdAsync(id);

            if (interview == null)
                return NotFound(new { message = "Interview not found." });

            return Ok(interview);
        }

        [Authorize(Roles = RoleConstants.CompanyHR)]
        [HttpPost]
        public async Task<IActionResult> Schedule(
            CreateInterviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            try
            {
                var interview = await _service.ScheduleAsync(request, userId);
                return Ok(interview);
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

        [Authorize(Roles = RoleConstants.CompanyHR + "," + RoleConstants.SuperAdmin)]
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            UpdateInterviewStatusRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            bool isSuperAdmin = User.IsInRole(RoleConstants.SuperAdmin);

            try
            {
                var interview = await _service.UpdateStatusAsync(
                    id, request, userId, isSuperAdmin);

                return Ok(interview);
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