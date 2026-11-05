using CampusPlacementManagement.DTOs.CompanyHRs;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/company-hrs")]
    public class CompanyHRsController : ControllerBase
    {
        private readonly ICompanyHRService _companyHRService;

        public CompanyHRsController(ICompanyHRService companyHRService)
        {
            _companyHRService = companyHRService;
        }

        // REGISTER COMPANY HR
        [HttpPost("register")]
        public async Task<IActionResult> RegisterCompanyHR(
            CreateCompanyHRRequest request)
        {
            try
            {
                var companyHR = await _companyHRService.RegisterAsync(request);

                return CreatedAtAction(
                    nameof(GetCompanyHRById),
                    new { id = companyHR.CompanyHRId },
                    companyHR);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET ALL COMPANY HRs
        [HttpGet]
        public async Task<IActionResult> GetAllCompanyHRs()
        {
            var companyHRs = await _companyHRService.GetAllAsync();

            return Ok(companyHRs);
        }

        // GET PENDING COMPANY HRs
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingCompanyHRs()
        {
            var companyHRs = await _companyHRService.GetPendingAsync();

            return Ok(companyHRs);
        }

        // GET COMPANY HR BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyHRById(int id)
        {
            var companyHR = await _companyHRService.GetByIdAsync(id);

            if (companyHR == null)
                return NotFound(new { message = "Company HR not found." });

            return Ok(companyHR);
        }

        // APPROVE COMPANY HR
        // approvedBy is temporarily supplied through the query string.
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveCompanyHR(
            int id,
            [FromQuery] string approvedBy)
        {
            if (string.IsNullOrWhiteSpace(approvedBy))
                return BadRequest(new { message = "Approver user ID is required." });

            var result = await _companyHRService.ApproveAsync(id, approvedBy);

            if (!result)
                return BadRequest(new
                {
                    message = "Company HR not found or is not pending approval."
                });

            return Ok(new { message = "Company HR approved successfully." });
        }

        // REJECT COMPANY HR
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectCompanyHR(int id)
        {
            var result = await _companyHRService.RejectAsync(id);

            if (!result)
                return BadRequest(new
                {
                    message = "Company HR not found or is not pending approval."
                });

            return Ok(new { message = "Company HR rejected successfully." });
        }

        // GET CURRENT COMPANY HR
        // Temporary userId query parameter for testing before authentication.
        [HttpGet("me")]
        public async Task<IActionResult> GetMyCompanyHR(
            [FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new { message = "User ID is required." });

            var companyHR = await _companyHRService.GetByUserIdAsync(userId);

            if (companyHR == null)
                return NotFound(new { message = "Company HR profile not found." });

            return Ok(companyHR);
        }
    }
}