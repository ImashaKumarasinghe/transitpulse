// ================================
// USING STATEMENTS (IMPORTS)
// ================================

using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT authentication
using Microsoft.EntityFrameworkCore;                 // EF Core (database)
using Microsoft.IdentityModel.Tokens;                // Token validation
using Microsoft.OpenApi;                             // Swagger / OpenAPI types
using System.Text;                                   // Encoding for JWT key
using TransitPulse.API.Data;                         // DbContext
using TransitPulse.API.Services;                     // AuthService

// ================================
// CREATE APPLICATION BUILDER
// ================================

var builder = WebApplication.CreateBuilder(args);

// ================================
// ADD SERVICES
// ================================

// Enable controllers (API endpoints)
builder.Services.AddControllers();

// Required for Swagger
builder.Services.AddEndpointsApiExplorer();

// ================================
// SWAGGER + JWT CONFIGURATION
// ================================

builder.Services.AddSwaggerGen(options =>
{
    // Basic Swagger information
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TransitPulse API",
        Version = "v1"
    });

    // Define JWT authentication in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer your_token_here"
    });

    // Apply JWT authentication to Swagger requests
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// ================================
// DATABASE CONFIGURATION
// ================================

// Register DbContext (connects to PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ================================
// REGISTER CUSTOM SERVICES
// ================================

// Register AuthService for dependency injection
builder.Services.AddScoped<IAuthService, AuthService>();

// ================================
// JWT AUTHENTICATION CONFIGURATION
// ================================

builder.Services.AddAuthentication(options =>
{
    // Set JWT as default authentication method
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Get secret key from appsettings.json
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

    // Define how JWT should be validated
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

// ================================
// ENABLE AUTHORIZATION
// ================================

builder.Services.AddAuthorization();

// ================================
// BUILD APPLICATION
// ================================

var app = builder.Build();

// ================================
// MIDDLEWARE PIPELINE
// ================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Run application
app.Run();