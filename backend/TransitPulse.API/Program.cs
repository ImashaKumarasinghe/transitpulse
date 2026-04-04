using Microsoft.EntityFrameworkCore;        // Needed for DbContext and EF Core
using TransitPulse.API.Data;               // Import your AppDbContext

// Create the builder (this is where we configure services)
var builder = WebApplication.CreateBuilder(args);


// ADD SERVICES TO THE CONTAINER


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
app.UseAuthorization();

// Map controllers to routes
// Example: /api/users, /api/routes
app.MapControllers();


// RUN THE APPLICATION


app.Run();