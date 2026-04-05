using TransitPulse.API.DTOs;

namespace TransitPulse.API.Services
{
    // Interface defines WHAT the service should do
    public interface IAuthService
    {
        // Register a user and return JWT token (or null if fail)
        Task<string?> RegisterAsync(RegisterDto dto);

        // Login user and return JWT token (or null if fail)
        Task<string?> LoginAsync(LoginDto dto);
    }
}