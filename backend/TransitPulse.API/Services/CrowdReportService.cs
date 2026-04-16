using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TransitPulse.API.Data;
using TransitPulse.API.DTOs;
using TransitPulse.API.Hubs;
using TransitPulse.API.Models;
using TransitPulse.API.Repositories;

namespace TransitPulse.API.Services
{
    public class CrowdReportService : ICrowdReportService
    {
        private readonly ICrowdReportRepository _reportRepo;
        private readonly AppDbContext _context;
        private readonly IHubContext<RouteHub> _hubContext;

        public CrowdReportService(
            ICrowdReportRepository reportRepo,
            AppDbContext context,
            IHubContext<RouteHub> hubContext)
        {
            _reportRepo = reportRepo;
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SubmitReportAsync(int userId, CreateCrowdReportDto dto)
        {
            // 1. Save report
            var report = new CrowdReport
            {
                UserId = userId,
                RouteId = dto.RouteId,
                CrowdLevel = dto.CrowdLevel,
                ReportedLocation = dto.ReportedLocation,
                ReportedAt = DateTime.UtcNow
            };

            await _reportRepo.AddAsync(report);

            // 2. Get recent reports
            var reports = await _reportRepo.GetRecentReportsAsync(dto.RouteId);

            // 3. Calculate crowd level
            var crowdLevel = CalculateCrowdLevel(reports);

            // 4. Update CurrentRouteStatus
            var status = await _context.CurrentRouteStatuses
                .FirstOrDefaultAsync(s => s.RouteId == dto.RouteId);

            if (status == null)
            {
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
                status.CurrentCrowdLevel = crowdLevel;
                status.LastUpdated = DateTime.UtcNow;
                status.RecentReportCount = reports.Count;
            }

            await _context.SaveChangesAsync();

            // 5. Send real-time update to clients in the correct route group
            var routeGroupName = $"route-{dto.RouteId}";

            var updateDto = new RouteStatusUpdateDto
            {
                RouteId = dto.RouteId,
                CurrentCrowdLevel = status.CurrentCrowdLevel,
                LastUpdated = status.LastUpdated,
                RecentReportCount = status.RecentReportCount
            };

            await _hubContext.Clients
                .Group(routeGroupName)
                .SendAsync("ReceiveRouteStatusUpdate", updateDto);
        }

        private string CalculateCrowdLevel(List<CrowdReport> reports)
        {
            if (reports.Count == 0)
                return "Low";

            var mostCommon = reports
                .GroupBy(r => r.CrowdLevel)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;

            return mostCommon;
        }
    }
}