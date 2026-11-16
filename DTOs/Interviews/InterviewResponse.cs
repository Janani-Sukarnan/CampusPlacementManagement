namespace CampusPlacementManagement.DTOs.Interviews
{
    public class InterviewResponse
    {
        public int InterviewId { get; set; }

        public int JobApplicationId { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public DateTime InterviewDate { get; set; }

        public string InterviewRound { get; set; } = string.Empty;

        public string InterviewMode { get; set; } = string.Empty;

        public string? Location { get; set; }

        public string? MeetingLink { get; set; }

        public string? Feedback { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}