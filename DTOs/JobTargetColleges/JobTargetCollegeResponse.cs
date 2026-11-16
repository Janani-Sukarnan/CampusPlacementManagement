namespace CampusPlacementManagement.DTOs.JobTargetColleges
{
    public class JobTargetCollegeResponse
    {
        public int JobTargetCollegeId { get; set; }

        public int JobOpeningId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public int CollegeId { get; set; }

        public string CollegeName { get; set; } = string.Empty;

        public DateTime TargetedAt { get; set; }
    }
}