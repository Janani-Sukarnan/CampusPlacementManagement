using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Models
{
    public class Interview
    {
        public int InterviewId { get; set; }

        public int JobApplicationId { get; set; }

        public DateTime InterviewDate { get; set; }

        public string InterviewRound { get; set; } = string.Empty;

        public string InterviewMode { get; set; } = string.Empty;

        public string? Location { get; set; }

        public string? MeetingLink { get; set; }

        public string? Feedback { get; set; }

        public InterviewStatus Status { get; set; } =
            InterviewStatus.Scheduled;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public JobApplication? JobApplication { get; set; }
    }
}