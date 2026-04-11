using TransitPulse.API.Models; // Import CrowdReport model

namespace TransitPulse.API.Repositories
{
    // This interface defines the contract for CrowdReport database operations
    public interface ICrowdReportRepository
    {
        // Add a new crowd report to the database
        // Example: User reports "High crowd" for Route 138
        Task<CrowdReport> AddAsync(CrowdReport report);

        // Get recent crowd reports for a specific route
        // Example: Get latest reports for Route 138
        Task<List<CrowdReport>> GetRecentReportsAsync(int routeId);
    }
}