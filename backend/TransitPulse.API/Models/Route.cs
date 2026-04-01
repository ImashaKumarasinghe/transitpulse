using System.ComponentModel.DataAnnotations;

namespace TransitPulse.API.Models
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        [Required]
        [MaxLength(20)]
        public string RouteNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string RouteName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string StartLocation { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string EndLocation { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string TransportType { get; set; } = string.Empty; // Bus or Train

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<CrowdReport> CrowdReports { get; set; } = new List<CrowdReport>();
        public CurrentRouteStatus? CurrentRouteStatus { get; set; }
    }
}