
namespace CampusPlacementManagement.DTOs.RecruitmentDrives
{
    public class RecruitmentDriveResponse
    {
        public int RecruitmentDriveId { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string DriveName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Venue { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
