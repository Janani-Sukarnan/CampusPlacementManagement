
using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.DTOs.JobApplications
{
    public class UpdateJobApplicationStatusRequest
    {
        [Required]
        [EnumDataType(typeof(ApplicationStatus))]
        public ApplicationStatus Status { get; set; }
    }
}
