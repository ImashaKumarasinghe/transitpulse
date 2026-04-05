using Microsoft.EntityFrameworkCore;        // Needed for DbContext and EF Core
using TransitPulse.API.Data;               // Import your AppDbContext
using TransitPulse.API.Services;           // Import your AuthService and IAuthService
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Create the builder (this is where we configure services)
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


// ADD SERVICES TO THE CONTAINER
builder.Services.AddScoped<IAuthService, AuthService>();

// Enable controllers (API endpoints like /api/users)
builder.Services.AddControllers();

// Enable Swagger (API testing UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// REGISTER DATABASE (VERY IMPORTANT)

// This tells the app:
// "Use AppDbContext as database manager"
// "Connect it to PostgreSQL using connection string from appsettings.json"
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// BUILD THE APPLICATION

var app = builder.Build();


// CONFIGURE MIDDLEWARE PIPELINE

// If app is running in Development mode
// enable Swagger UI (so you can test APIs easily)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP → HTTPS (security)
app.UseHttpsRedirection();

// Enable authorization (used later with JWT)
app.UseAuthentication();
app.UseAuthorization();

// Map controllers to routes
// Example: /api/users, /api/routes
app.MapControllers();


// RUN THE APPLICATION


app.Run();