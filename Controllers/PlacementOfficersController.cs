
using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/placement-officers")]
    [ApiController]
    public class PlacementOfficersController : ControllerBase
    {
        private readonly IPlacementOfficerService _service;

        public PlacementOfficersController(IPlacementOfficerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var officers = await _service.GetAllAsync();
            return Ok(officers);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var officers = await _service.GetPendingAsync();
            return Ok(officers);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var officer = await _service.GetByIdAsync(id);

            if (officer == null)
                return NotFound(new { message = "Placement officer not found." });

            return Ok(officer);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User is not authenticated." });

            var officer = await _service.GetByUserIdAsync(userId);

            if (officer == null)
                return NotFound(new { message = "Placement officer profile not found." });

            return Ok(officer);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            CreatePlacementOfficerRequest request)
        {
            try
            {
                var officer = await _service.RegisterAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = officer.PlacementOfficerId },
                    officer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var approvedByUserId =
                    User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _service.ApproveAsync(
                    id, approvedByUserId);

                if (!result)
                    return NotFound(new { message = "Placement officer not found." });

                return Ok(new { message = "Placement officer approved successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _service.RejectAsync(id);

            if (!result)
                return NotFound(new { message = "Placement officer not found." });

            return Ok(new { message = "Placement officer rejected successfully." });
        }

        [HttpPut("{id:int}/suspend")]
        public async Task<IActionResult> Suspend(int id)
        {
            var result = await _service.SuspendAsync(id);

            if (!result)
                return NotFound(new { message = "Placement officer not found." });

            return Ok(new { message = "Placement officer suspended successfully." });
        }
    }
}
