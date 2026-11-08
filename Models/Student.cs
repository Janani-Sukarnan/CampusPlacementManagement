using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int CollegeId { get; set; }

        public string RegistrationNumber { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Degree { get; set; } = string.Empty;

        public int GraduationYear { get; set; }

        public decimal CGPA { get; set; }

        public string? ResumeUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }

        public College? College { get; set; }
    }
}