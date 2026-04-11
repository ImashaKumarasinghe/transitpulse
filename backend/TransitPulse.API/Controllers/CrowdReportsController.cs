using Microsoft.AspNetCore.Authorization; // For authentication/authorization
using Microsoft.AspNetCore.Mvc;           // For API controller features
using System.Security.Claims;             // To extract user info from JWT
using TransitPulse.API.DTOs;              // DTOs (input data)
using TransitPulse.API.Services;          // Service layer

namespace TransitPulse.API.Controllers
{
    // Marks this class as an API controller
    [ApiController]

    // Base route: api/crowdreports
    [Route("api/[controller]")]
    public class CrowdReportsController : ControllerBase
    {
        // Service dependency (business logic)
        private readonly ICrowdReportService _service;

        // Constructor - Dependency Injection
        public CrowdReportsController(ICrowdReportService service)
        {
            _service = service;
        }

        // POST: api/crowdreports
        // Only logged-in users can submit a report
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitReport(CreateCrowdReportDto dto)
        {
            // Extract userId from JWT token
            // ClaimTypes.NameIdentifier usually stores UserId
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // Call service to handle report submission
            await _service.SubmitReportAsync(userId, dto);

            // Return success response
            return Ok("Report submitted successfully");
        }
    }
}