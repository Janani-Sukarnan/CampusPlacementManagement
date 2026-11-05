
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Models
{
    public class PlacementOfficer
    {
        public int PlacementOfficerId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int CollegeId { get; set; }

        public string Designation { get; set; } = string.Empty;

        public AccountStatus Status { get; set; } = AccountStatus.Pending;

        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }

        public College? College { get; set; }

        public ApplicationUser? Approver { get; set; }
    }
}
