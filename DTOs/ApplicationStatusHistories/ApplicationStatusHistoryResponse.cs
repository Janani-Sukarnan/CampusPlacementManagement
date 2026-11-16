namespace CampusPlacementManagement.DTOs.ApplicationStatusHistories
{
    public class ApplicationStatusHistoryResponse
    {
        public int ApplicationStatusHistoryId { get; set; }

        public int JobApplicationId { get; set; }

        public string PreviousStatus { get; set; } = string.Empty;

        public string NewStatus { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }

        public string ChangedByUserId { get; set; } = string.Empty;

        public string ChangedByName { get; set; } = string.Empty;

        public string? Remarks { get; set; }
    }
}