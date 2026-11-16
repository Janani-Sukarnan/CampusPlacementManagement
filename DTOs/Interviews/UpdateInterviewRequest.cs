using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.DTOs.Interviews
{
    public class UpdateInterviewStatusRequest
    {
        [Required]
        [EnumDataType(typeof(InterviewStatus))]
        public InterviewStatus Status { get; set; }

        [MaxLength(2000)]
        public string? Feedback { get; set; }
    }
}