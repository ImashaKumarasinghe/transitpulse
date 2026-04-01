using System.ComponentModel.DataAnnotations; // Used for validation attributes (like Required, MaxLength)

namespace TransitPulse.API.Models
{
    // This class represents the "Users" table in the database
    public class User
    {
        // PRIMARY KEY (unique identifier for each user)
        [Key]
        public int UserId { get; set; }

        // Full name of the user
        // Required → cannot be empty
        // MaxLength → max 100 characters
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        // Email address of the user
        // Required → must be provided
        // MaxLength → limit to 100 characters
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        // Stores hashed password (NOT plain password)
        // Always store encrypted/hashed values for security
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Role of the user (e.g., "User", "Admin")
        // Default value is "User"
        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User";

        // Stores when the user account was created
        // Automatically set to current UTC time
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // NAVIGATION PROPERTY (relationship)
        // One User can have MANY CrowdReports
        // This helps Entity Framework understand relationships between tables
        public ICollection<CrowdReport> CrowdReports { get; set; } = new List<CrowdReport>();
    }
}