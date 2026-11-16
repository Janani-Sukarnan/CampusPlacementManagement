
namespace CampusPlacementManagement.DTOs.JobApplications
{
    public class JobApplicationResponse
    {
        public int JobApplicationId { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public int JobOpeningId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public DateTime AppliedAt { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? ResumeUrl { get; set; }

        public string? CoverLetter { get; set; }
    }
}
