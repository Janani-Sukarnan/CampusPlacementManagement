using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Interviews
{
    public class CreateInterviewRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int JobApplicationId { get; set; }

        [Required]
        public DateTime InterviewDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string InterviewRound { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string InterviewMode { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Location { get; set; }

        [MaxLength(500)]
        public string? MeetingLink { get; set; }
    }
}