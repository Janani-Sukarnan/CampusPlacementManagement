
using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegesController : ControllerBase
    {
        private readonly ICollegeService _collegeService;

        public CollegesController(ICollegeService collegeService)
        {
            _collegeService = collegeService;
        }

        // GET: api/colleges
        [HttpGet]
        public async Task<IActionResult> GetAllColleges()
        {
            var colleges = await _collegeService.GetAllCollegesAsync();

            return Ok(colleges);
        }

        // GET: api/colleges/pending
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingColleges()
        {
            var colleges = await _collegeService.GetPendingCollegesAsync();

            return Ok(colleges);
        }

        // GET: api/colleges/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCollegeById(int id)
        {
            var college = await _collegeService.GetCollegeByIdAsync(id);

            if (college == null)
            {
                return NotFound(new { message = "College not found." });
            }

            return Ok(college);
        }

        // POST: api/colleges/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterCollege(
            CreateCollegeRequest request)
        {
            try
            {
                var college = await _collegeService.CreateCollegeAsync(request);

                return CreatedAtAction(
                    nameof(GetCollegeById),
                    new { id = college.CollegeId },
                    college);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/colleges/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCollege(
            int id,
            UpdateCollegeRequest request)
        {
            try
            {
                var college = await _collegeService.UpdateCollegeAsync(id, request);

                if (college == null)
                {
                    return NotFound(new { message = "College not found." });
                }

                return Ok(college);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/colleges/5/approve
        [HttpPut("{id:int}/approve")]
        public async Task<IActionResult> ApproveCollege(int id)
        {
            var approved = await _collegeService.ApproveCollegeAsync(id);

            if (!approved)
            {
                return NotFound(new { message = "College not found." });
            }

            return Ok(new { message = "College approved successfully." });
        }

        // PUT: api/colleges/5/reject
        [HttpPut("{id:int}/reject")]
        public async Task<IActionResult> RejectCollege(int id)
        {
            var rejected = await _collegeService.RejectCollegeAsync(id);

            if (!rejected)
            {
                return NotFound(new { message = "College not found." });
            }

            return Ok(new { message = "College rejected successfully." });
        }

        // PUT: api/colleges/5/suspend
        [HttpPut("{id:int}/suspend")]
        public async Task<IActionResult> SuspendCollege(int id)
        {
            var suspended = await _collegeService.SuspendCollegeAsync(id);

            if (!suspended)
            {
                return NotFound(new { message = "College not found." });
            }

            return Ok(new { message = "College suspended successfully." });
        }
    }
}
