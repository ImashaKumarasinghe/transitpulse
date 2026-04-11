using TransitPulse.API.DTOs; // Import DTOs (CreateCrowdReportDto)

namespace TransitPulse.API.Services
{
    // This interface defines what operations are available
    // for handling crowd reporting business logic
    public interface ICrowdReportService
    {
        // Method to submit a new crowd report
        // userId → comes from logged-in user (JWT token)
        // dto → contains report details (routeId, crowd level, etc.)
        Task SubmitReportAsync(int userId, CreateCrowdReportDto dto);
    }
}