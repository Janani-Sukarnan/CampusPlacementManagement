
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Models
{
    public class JobApplication
    {
        public int JobApplicationId { get; set; }

        public int StudentId { get; set; }

        public int JobOpeningId { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public ApplicationStatus Status { get; set; } =
            ApplicationStatus.Applied;

        public string? ResumeUrl { get; set; }

        public string? CoverLetter { get; set; }

        // Navigation properties
        public Student? Student { get; set; }

        public JobOpening? JobOpening { get; set; }
    }
}
