
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models
{
    public class RecruitmentDrive
    {
        [Key]
        public int RecruitmentDriveId { get; set; }

        public int CompanyId { get; set; }

        [Required]
        [MaxLength(150)]
        public string DriveName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [MaxLength(200)]
        public string Venue { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Company Company { get; set; } = null!;
    }
}
