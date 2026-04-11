namespace TransitPulse.API.DTOs
{
    public class UpdateRouteDto
    {
        public string RouteNumber { get; set; } = string.Empty;
        public string RouteName { get; set; } = string.Empty;
        public string StartLocation { get; set; } = string.Empty;
        public string EndLocation { get; set; } = string.Empty;
        public string TransportType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}