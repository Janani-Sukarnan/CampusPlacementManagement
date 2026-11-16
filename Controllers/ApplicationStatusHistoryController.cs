
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/application-status-histories")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationStatusHistoriesController : ControllerBase
    {
        private readonly IApplicationStatusHistoryService _service;

        public ApplicationStatusHistoriesController(
            IApplicationStatusHistoryService service)
        {
            _service = service;
        }

        // GET: api/application-status-histories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var histories = await _service.GetAllAsync();
            return Ok(histories);
        }

        // GET: api/application-status-histories/job-application/1
        [HttpGet("job-application/{jobApplicationId}")]
        public async Task<IActionResult> GetByJobApplicationId(
            int jobApplicationId)
        {
            if (jobApplicationId <= 0)
                return BadRequest("Invalid job application ID.");

            var histories =
                await _service.GetByJobApplicationIdAsync(jobApplicationId);

            return Ok(histories);
        }
    }
}
