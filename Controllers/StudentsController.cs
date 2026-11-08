
using System.Security.Claims;
using CampusPlacementManagement.DTOs.Students;
using CampusPlacementManagement.Helpers;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // PUBLIC: Student registration
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            CreateStudentRequest request)
        {
            try
            {
                var student = await _studentService.RegisterAsync(request);
                return Ok(student);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // SUPERADMIN: Get all students
        [Authorize(Roles = RoleConstants.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        // SUPERADMIN: Get pending students
        [Authorize(Roles = RoleConstants.SuperAdmin)]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var students = await _studentService.GetPendingAsync();
            return Ok(students);
        }

        // SUPERADMIN: Get student by ID
        [Authorize(Roles = RoleConstants.SuperAdmin)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);

            if (student == null)
                return NotFound(new { message = "Student not found." });

            return Ok(student);
        }

        // STUDENT: Get own profile
        [Authorize(Roles = RoleConstants.Student)]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var student = await _studentService.GetByUserIdAsync(userId);

            if (student == null)
                return NotFound(new { message = "Student profile not found." });

            return Ok(student);
        }

        // STUDENT: Update own profile
        [Authorize(Roles = RoleConstants.Student)]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(
            UpdateStudentRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var existingStudent =
                await _studentService.GetByUserIdAsync(userId);

            if (existingStudent == null)
                return NotFound(new { message = "Student profile not found." });

            var updatedStudent = await _studentService.UpdateAsync(
                existingStudent.StudentId, request);

            return Ok(updatedStudent);
        }

        // SUPERADMIN: Approve student
        [Authorize(Roles = RoleConstants.SuperAdmin)]
        [HttpPut("{id:int}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _studentService.ApproveAsync(id);

            if (!result)
                return BadRequest(new
                {
                    message = "Student not found or is not pending approval."
                });

            return Ok(new { message = "Student approved successfully." });
        }

        // SUPERADMIN: Reject student
        [Authorize(Roles = RoleConstants.SuperAdmin)]
        [HttpPut("{id:int}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _studentService.RejectAsync(id);

            if (!result)
                return BadRequest(new
                {
                    message = "Student not found or is not pending approval."
                });

            return Ok(new { message = "Student rejected successfully." });
        }
    }
}
