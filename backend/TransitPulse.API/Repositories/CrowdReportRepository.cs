using Microsoft.EntityFrameworkCore; // EF Core methods (ToListAsync, Where, etc.)
using TransitPulse.API.Data;         // AppDbContext (database connection)
using TransitPulse.API.Models;       // CrowdReport model

namespace TransitPulse.API.Repositories
{
    // This class implements ICrowdReportRepository
    public class CrowdReportRepository : ICrowdReportRepository
    {
        // Database context (used to access tables)
        private readonly AppDbContext _context;

        // Constructor - Dependency Injection
        // ASP.NET automatically provides AppDbContext
        public CrowdReportRepository(AppDbContext context)
        {
            _context = context;
        }

        // Add a new crowd report to the database
        public async Task<CrowdReport> AddAsync(CrowdReport report)
        {
            // Add the report to CrowdReports table
            _context.CrowdReports.Add(report);

            // Save changes to database
            await _context.SaveChangesAsync();

            // Return saved report (with ID, timestamp, etc.)
            return report;
        }

        // Get recent crowd reports for a specific route
        public async Task<List<CrowdReport>> GetRecentReportsAsync(int routeId)
        {
            // Get current time and subtract 1 hour
            // This means we only consider reports from the last 1 hour
            var oneHourAgo = DateTime.UtcNow.AddHours(-1);

            // Query database:
            // 1. Filter by RouteId
            // 2. Filter reports within last 1 hour
            return await _context.CrowdReports
                .Where(r => r.RouteId == routeId && r.ReportedAt >= oneHourAgo)
                .ToListAsync();
        }
    }
}