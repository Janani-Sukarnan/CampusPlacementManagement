
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Models
{
    public class JobOpening
    {
        public int JobOpeningId { get; set; }

        public int CompanyId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string EmploymentType { get; set; } = string.Empty;

        public decimal MinimumCGPA { get; set; }

        public string? SalaryRange { get; set; }

        public DateTime ApplicationDeadline { get; set; }

        public JobOpeningStatus Status { get; set; } =
            JobOpeningStatus.Open;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public Company? Company { get; set; }
    }
}
