
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.DTOs.JobOpenings
{
    public class JobOpeningResponse
    {
        public int JobOpeningId { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string EmploymentType { get; set; } = string.Empty;

        public decimal MinimumCGPA { get; set; }

        public string? SalaryRange { get; set; }

        public DateTime ApplicationDeadline { get; set; }

        public JobOpeningStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
