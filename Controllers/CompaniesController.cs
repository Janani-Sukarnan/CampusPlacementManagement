
using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authorization;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // Register a company
        [HttpPost("register")]
        public async Task<IActionResult> RegisterCompany(
            CreateCompanyRequest request)
        {
            try
            {
                var company = await _companyService.RegisterCompanyAsync(request);
                return CreatedAtAction(
                    nameof(GetCompanyById),
                    new { id = company.CompanyId },
                    company);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // Get all companies
        [HttpGet]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        // Get pending companies
        [HttpGet("pending")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetPendingCompanies()
        {
            var companies = await _companyService.GetPendingCompaniesAsync();
            return Ok(companies);
        }

        // Get company by ID
        [HttpGet("{id}")]
        //[Authorize(Roles = "SuperAdmin,CompanyHR")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);

            if (company == null)
                return NotFound(new { message = "Company not found." });

            return Ok(company);
        }

        // Update company
        [HttpPut("{id}")]
        //[Authorize(Roles = "SuperAdmin,CompanyHR")]
        public async Task<IActionResult> UpdateCompany(
            int id, UpdateCompanyRequest request)
        {
            try
            {
                var company = await _companyService.UpdateCompanyAsync(id, request);
                return Ok(company);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Approve company
        [HttpPut("{id}/approve")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ApproveCompany(int id)
        {
            var result = await _companyService.ApproveCompanyAsync(id);

            if (!result)
                return BadRequest(new
                {
                    message = "Company not found or cannot be approved."
                });

            return Ok(new { message = "Company approved successfully." });
        }

        // Reject company
        [HttpPut("{id}/reject")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RejectCompany(int id)
        {
            var result = await _companyService.RejectCompanyAsync(id);

            if (!result)
                return BadRequest(new
                {
                    message = "Company not found or cannot be rejected."
                });

            return Ok(new { message = "Company rejected successfully." });
        }

        // Suspend company
        [HttpPut("{id}/suspend")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SuspendCompany(int id)
        {
            var result = await _companyService.SuspendCompanyAsync(id);

            if (!result)
                return BadRequest(new
                {
                    message = "Company not found or cannot be suspended."
                });

            return Ok(new { message = "Company suspended successfully." });
        }
    }
}
