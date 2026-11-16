
using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.JobApplications
{
    public class CreateJobApplicationRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int JobOpeningId { get; set; }

        [MaxLength(500)]
        public string? ResumeUrl { get; set; }

        [MaxLength(2000)]
        public string? CoverLetter { get; set; }
    }
}
