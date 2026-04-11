using Microsoft.EntityFrameworkCore; // For database queries (FirstOrDefaultAsync)
using TransitPulse.API.Data;         // AppDbContext (DB connection)
using TransitPulse.API.DTOs;         // DTOs (input data)
using TransitPulse.API.Models;       // Models (CrowdReport, CurrentRouteStatus)
using TransitPulse.API.Repositories; // Repository interface

namespace TransitPulse.API.Services
{
    // This class implements ICrowdReportService
    public class CrowdReportService : ICrowdReportService
    {
        // Repository for crowd reports
        private readonly ICrowdReportRepository _reportRepo;

        // Direct DB context (used for CurrentRouteStatus table)
        private readonly AppDbContext _context;

        // Constructor - Dependency Injection
        public CrowdReportService(ICrowdReportRepository reportRepo, AppDbContext context)
        {
            _reportRepo = reportRepo;
            _context = context;
        }

        // Main method to submit a crowd report
        public async Task SubmitReportAsync(int userId, CreateCrowdReportDto dto)
        {
            // -------------------------------
            // 1. SAVE NEW REPORT
            // -------------------------------

            // Create CrowdReport object from input DTO
            var report = new CrowdReport
            {
                UserId = userId,                    // Who submitted the report
                RouteId = dto.RouteId,              // Which route
                CrowdLevel = dto.CrowdLevel,        // Crowd level (Low/Medium/High)
                ReportedLocation = dto.ReportedLocation, // Optional location
                ReportedAt = DateTime.UtcNow        // Current time (important for filtering)
            };

            // Save report in database
            await _reportRepo.AddAsync(report);

            // -------------------------------
            // 2. GET RECENT REPORTS
            // -------------------------------

            // Get reports from last 1 hour (repository logic)
            var reports = await _reportRepo.GetRecentReportsAsync(dto.RouteId);

            // -------------------------------
            // 3. CALCULATE CURRENT CROWD LEVEL
            // -------------------------------

            // Determine overall crowd level based on recent reports
            var crowdLevel = CalculateCrowdLevel(reports);

            // -------------------------------
            // 4. UPDATE CURRENT ROUTE STATUS
            // -------------------------------

            // Check if status already exists for this route
            var status = await _context.CurrentRouteStatuses
                .FirstOrDefaultAsync(s => s.RouteId == dto.RouteId);

            if (status == null)
            {
                // If no status exists → create new one
                status = new CurrentRouteStatus
                {
                    RouteId = dto.RouteId,
                    CurrentCrowdLevel = crowdLevel,
                    LastUpdated = DateTime.UtcNow,
                    RecentReportCount = reports.Count
                };

                _context.CurrentRouteStatuses.Add(status);
            }
            else
            {
                // If exists → update existing record
                status.CurrentCrowdLevel = crowdLevel;
                status.LastUpdated = DateTime.UtcNow;
                status.RecentReportCount = reports.Count;
            }

            // Save status changes
            await _context.SaveChangesAsync();
        }

        // ----------------------------------------
        // HELPER METHOD: CALCULATE CROWD LEVEL
        // ----------------------------------------
        private string CalculateCrowdLevel(List<CrowdReport> reports)
        {
            // If no reports → default to Low
            if (reports.Count == 0)
                return "Low";

            // Extract all crowd levels from reports
            var levels = reports.Select(r => r.CrowdLevel).ToList();

            // Find the most frequently occurring crowd level
            var mostCommon = levels
                .GroupBy(x => x)              // Group same values
                .OrderByDescending(g => g.Count()) // Sort by count (highest first)
                .First()                     // Take top group
                .Key;                        // Get the value (Low/Medium/High)

            return mostCommon;
        }
    }
}