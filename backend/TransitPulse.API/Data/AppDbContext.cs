using Microsoft.EntityFrameworkCore; // EF Core classes like DbContext, DbSet, ModelBuilder
using TransitPulse.API.Models;       // Import entity/model classes

namespace TransitPulse.API.Data
{
    // AppDbContext is the main bridge between the C# app and the PostgreSQL database
    public class AppDbContext : DbContext
    {
        // Constructor receives database configuration from Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Each DbSet<T> represents a table in the database

        // Users table
        public DbSet<User> Users { get; set; }

        // Routes table
        public DbSet<Models.Route> Routes { get; set; }

        // CrowdReports table
        public DbSet<CrowdReport> CrowdReports { get; set; }

        // CurrentRouteStatuses table
        public DbSet<CurrentRouteStatus> CurrentRouteStatuses { get; set; }

        // This method is used to define extra database rules and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Make User.Email unique
            // This means two users cannot register using the same email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // One Route can have many CrowdReports
            // Each CrowdReport belongs to one Route
            modelBuilder.Entity<CrowdReport>()
                .HasOne(cr => cr.Route)
                .WithMany(r => r.CrowdReports)
                .HasForeignKey(cr => cr.RouteId)
                .OnDelete(DeleteBehavior.Cascade);

            // One User can have many CrowdReports
            // Each CrowdReport belongs to one User
            modelBuilder.Entity<CrowdReport>()
                .HasOne(cr => cr.User)
                .WithMany(u => u.CrowdReports)
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One Route has one CurrentRouteStatus
            // One CurrentRouteStatus belongs to one Route
            modelBuilder.Entity<CurrentRouteStatus>()
                .HasOne(cs => cs.Route)
                .WithOne(r => r.CurrentRouteStatus)
                .HasForeignKey<CurrentRouteStatus>(cs => cs.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}