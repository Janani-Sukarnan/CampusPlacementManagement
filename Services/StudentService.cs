
using CampusPlacementManagement.DTOs.Students;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Helpers;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICollegeRepository _collegeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public StudentService(
            IStudentRepository studentRepository,
            ICollegeRepository collegeRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _studentRepository = studentRepository;
            _collegeRepository = collegeRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<StudentResponse>> GetAllAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(MapToResponse);
        }

        public async Task<IEnumerable<StudentResponse>> GetPendingAsync()
        {
            var students = await _studentRepository.GetPendingAsync();
            return students.Select(MapToResponse);
        }

        public async Task<StudentResponse?> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return student == null ? null : MapToResponse(student);
        }

        public async Task<StudentResponse?> GetByUserIdAsync(string userId)
        {
            var student = await _studentRepository.GetByUserIdAsync(userId);
            return student == null ? null : MapToResponse(student);
        }

        public async Task<StudentResponse> RegisterAsync(
            CreateStudentRequest request)
        {
            var college = await _collegeRepository
                .GetCollegeByIdAsync(request.CollegeId);

            if (college == null)
                throw new InvalidOperationException("College not found.");

            if (college.Status != CollegeStatus.Approved)
                throw new InvalidOperationException(
                    "Students can register only under an approved college.");

            if (await _studentRepository
                .ExistsByRegistrationNumberAsync(request.RegistrationNumber))
                throw new InvalidOperationException(
                    "Registration number already exists.");

            if (await _userManager.FindByEmailAsync(request.Email) != null)
                throw new InvalidOperationException(
                    "An account with this email already exists.");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                AccountStatus = AccountStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(
                user, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    createResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }

            try
            {
                if (!await _roleManager.RoleExistsAsync(RoleConstants.Student))
                {
                    var roleResult = await _roleManager.CreateAsync(
                        new IdentityRole(RoleConstants.Student));

                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(
                            "; ",
                            roleResult.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(errors);
                    }
                }

                var roleAssignment = await _userManager.AddToRoleAsync(
                    user, RoleConstants.Student);

                if (!roleAssignment.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleAssignment.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(errors);
                }

                var student = new Student
                {
                    UserId = user.Id,
                    CollegeId = request.CollegeId,
                    RegistrationNumber = request.RegistrationNumber,
                    Department = request.Department,
                    Degree = request.Degree,
                    GraduationYear = request.GraduationYear,
                    CGPA = request.CGPA,
                    ResumeUrl = request.ResumeUrl
                };

                await _studentRepository.AddAsync(student);

                var savedStudent = await _studentRepository
                    .GetByIdAsync(student.StudentId);

                if (savedStudent == null)
                    throw new InvalidOperationException(
                        "Unable to retrieve the registered student.");

                return MapToResponse(savedStudent);
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                throw;
            }
        }

        public async Task<StudentResponse?> UpdateAsync(
            int id, UpdateStudentRequest request)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
                return null;

            student.User!.FullName = request.FullName;
            student.User.PhoneNumber = request.PhoneNumber;
            student.Department = request.Department;
            student.Degree = request.Degree;
            student.GraduationYear = request.GraduationYear;
            student.CGPA = request.CGPA;
            student.ResumeUrl = request.ResumeUrl;
            student.UpdatedAt = DateTime.UtcNow;
            student.User.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(student.User);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }

            await _studentRepository.UpdateAsync(student);

            return MapToResponse(student);
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null ||
                student.User == null ||
                student.User.AccountStatus != AccountStatus.Pending)
                return false;

            student.User.AccountStatus = AccountStatus.Approved;
            student.User.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(student.User);
            return result.Succeeded;
        }

        public async Task<bool> RejectAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null ||
                student.User == null ||
                student.User.AccountStatus != AccountStatus.Pending)
                return false;

            student.User.AccountStatus = AccountStatus.Rejected;
            student.User.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(student.User);
            return result.Succeeded;
        }

        private static StudentResponse MapToResponse(Student student)
        {
            return new StudentResponse
            {
                StudentId = student.StudentId,
                UserId = student.UserId,
                FullName = student.User?.FullName ?? string.Empty,
                Email = student.User?.Email ?? string.Empty,
                PhoneNumber = student.User?.PhoneNumber ?? string.Empty,
                CollegeId = student.CollegeId,
                CollegeName = student.College?.CollegeName ?? string.Empty,
                RegistrationNumber = student.RegistrationNumber,
                Department = student.Department,
                Degree = student.Degree,
                GraduationYear = student.GraduationYear,
                CGPA = student.CGPA,
                ResumeUrl = student.ResumeUrl,
                CreatedAt = student.CreatedAt
            };
        }
    }
}
