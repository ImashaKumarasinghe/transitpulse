using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransitPulse.API.Models
{
    public class CrowdReport
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RouteId { get; set; }

        [Required]
        [MaxLength(20)]
        public string CrowdLevel { get; set; } = string.Empty; // Low, Medium, High, Very High

        [Required]
        [MaxLength(100)]
        public string ReportedLocation { get; set; } = string.Empty;

        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("RouteId")]
        public Route? Route { get; set; }
    }
}