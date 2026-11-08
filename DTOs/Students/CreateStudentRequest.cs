using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Students
{
    public class CreateStudentRequest
    {
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public int CollegeId { get; set; }

        [Required]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        [Required]
        public string Degree { get; set; } = string.Empty;

        [Required]
        public int GraduationYear { get; set; }

        [Range(0, 10)]
        public decimal CGPA { get; set; }

        public string? ResumeUrl { get; set; }
    }
}