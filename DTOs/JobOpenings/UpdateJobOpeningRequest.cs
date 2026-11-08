
using System.ComponentModel.DataAnnotations;
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.DTOs.JobOpenings
{
    public class UpdateJobOpeningRequest
    {
        [Required]
        [MaxLength(150)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string JobDescription { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string EmploymentType { get; set; } = string.Empty;

        [Range(0, 10)]
        public decimal MinimumCGPA { get; set; }

        [MaxLength(100)]
        public string? SalaryRange { get; set; }

        [Required]
        public DateTime ApplicationDeadline { get; set; }

        public JobOpeningStatus Status { get; set; }
    }
}
