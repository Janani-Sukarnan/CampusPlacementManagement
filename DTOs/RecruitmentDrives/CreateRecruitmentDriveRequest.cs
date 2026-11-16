
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.RecruitmentDrives
{
    public class CreateRecruitmentDriveRequest
    {
        [Required]
        [MaxLength(150)]
        public string DriveName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string Venue { get; set; } = string.Empty;
    }
}
