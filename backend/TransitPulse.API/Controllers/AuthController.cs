using Microsoft.AspNetCore.Mvc;              // For building API controllers
using TransitPulse.API.DTOs;                 // For RegisterDto & LoginDto
using TransitPulse.API.Services;             // For IAuthService

namespace TransitPulse.API.Controllers
{
    // Marks this class as an API controller (enables automatic validation, binding, etc.)
    [ApiController]

    // Base route: api/auth
    // [controller] = Auth (controller name without "Controller")
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // This is the service that contains authentication logic
        private readonly IAuthService _authService;

        // Constructor (Dependency Injection)
        // .NET will automatically provide AuthService because we registered it in Program.cs
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // =========================
        // REGISTER ENDPOINT
        // =========================
        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // Call service to register user
            var token = await _authService.RegisterAsync(dto);

            // If email already exists, service returns null
            if (token == null)
                return BadRequest("Email already exists");

            // If successful, return JWT token
            return Ok(new { token });
        }

        // =========================
        // LOGIN ENDPOINT
        // =========================
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // Call service to login user
            var token = await _authService.LoginAsync(dto);

            // If login fails (wrong email or password)
            if (token == null)
                return Unauthorized("Invalid credentials");

            // If successful, return JWT token
            return Ok(new { token });
        }
    }
}