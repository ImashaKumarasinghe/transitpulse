using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransitPulse.API.Models
{
    public class CurrentRouteStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        public int RouteId { get; set; }

        [Required]
        [MaxLength(20)]
        public string CurrentCrowdLevel { get; set; } = "Low";

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public int RecentReportCount { get; set; } = 0;

        [ForeignKey("RouteId")]
        public Route? Route { get; set; }
    }
}