using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models
{
    public class JobTargetCollege
    {
        [Key]
        public int JobTargetCollegeId { get; set; }

        public int JobOpeningId { get; set; }

        public int CollegeId { get; set; }

        public DateTime TargetedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public JobOpening JobOpening { get; set; } = null!;

        public College College { get; set; } = null!;
    }
}