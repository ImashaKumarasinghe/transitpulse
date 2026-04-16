namespace TransitPulse.API.DTOs
{
    public class RouteStatusUpdateDto
    {
        public int RouteId { get; set; }
        public string CurrentCrowdLevel { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
        public int RecentReportCount { get; set; }
    }
}