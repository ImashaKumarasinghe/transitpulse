namespace TransitPulse.API.DTOs
{
    public class CreateCrowdReportDto
    {
        public int RouteId { get; set; }
        public string CrowdLevel { get; set; } = string.Empty;
        public string ReportedLocation { get; set; } = string.Empty;
    }
}