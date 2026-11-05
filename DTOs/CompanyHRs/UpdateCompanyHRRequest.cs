using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.CompanyHRs
{
    public class UpdateCompanyHRRequest
    {
        [Required]
        public int CompanyId { get; set; }

        [MaxLength(100)]
        public string? Designation { get; set; }
    }
}