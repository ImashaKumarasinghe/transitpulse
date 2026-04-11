using Microsoft.EntityFrameworkCore; // Needed for EF Core methods (ToListAsync, FirstOrDefaultAsync)
using TransitPulse.API.Data;         // Contains AppDbContext (database connection)
using TransitPulse.API.Models;       // Contains Route model
using Route = TransitPulse.API.Models.Route; // Disambiguate Route class

namespace TransitPulse.API.Repositories
{
    // This class implements IRouteRepository (contract)
    public class RouteRepository : IRouteRepository
    {
        // This is the database context (EF Core)
        private readonly AppDbContext _context;

        // Constructor - dependency injection
        // ASP.NET automatically provides AppDbContext here
        public RouteRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all routes from database
        public async Task<List<Route>> GetAllAsync()
        {
            // Fetch all rows from Routes table
            return await _context.Routes.ToListAsync();
        }

        // Get a single route by ID
        public async Task<Route?> GetByIdAsync(int id)
        {
            // Find first route where RouteId matches
            return await _context.Routes
                .FirstOrDefaultAsync(r => r.RouteId == id);
        }

        // Add a new route
        public async Task<Route> AddAsync(Route route)
        {
            // Add route object to database
            _context.Routes.Add(route);

            // Save changes to database
            await _context.SaveChangesAsync();

            // Return the inserted route (with ID)
            return route;
        }

        // Update existing route
        public async Task<Route?> UpdateAsync(Route route)
        {
            // Find route from database
            var existingRoute = await _context.Routes
                .FirstOrDefaultAsync(r => r.RouteId == route.RouteId);

            // If not found, return null
            if (existingRoute == null)
                return null;

            // Update fields
            existingRoute.RouteNumber = route.RouteNumber;
            existingRoute.RouteName = route.RouteName;
            existingRoute.StartLocation = route.StartLocation;
            existingRoute.EndLocation = route.EndLocation;
            existingRoute.TransportType = route.TransportType;
            existingRoute.IsActive = route.IsActive;

            // Save updated data
            await _context.SaveChangesAsync();

            // Return updated route
            return existingRoute;
        }

        // Delete a route
        public async Task<bool> DeleteAsync(int id)
        {
            // Find route by ID
            var route = await _context.Routes
                .FirstOrDefaultAsync(r => r.RouteId == id);

            // If not found, return false
            if (route == null)
                return false;

            // Remove route from database
            _context.Routes.Remove(route);

            // Save changes
            await _context.SaveChangesAsync();

            return true;
        }
    }
}