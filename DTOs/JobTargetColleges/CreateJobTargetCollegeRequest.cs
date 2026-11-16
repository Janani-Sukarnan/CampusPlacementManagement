using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.JobTargetColleges
{
    public class CreateJobTargetCollegeRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int JobOpeningId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CollegeId { get; set; }
    }
}