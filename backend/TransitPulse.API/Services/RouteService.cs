using TransitPulse.API.DTOs;         // DTO classes like CreateRouteDto, UpdateRouteDto, RouteResponseDto
using TransitPulse.API.Models;       // Route model/entity
using TransitPulse.API.Repositories; // Repository interface
using RouteModel = TransitPulse.API.Models.Route;

namespace TransitPulse.API.Services
{
    // This class implements the IRouteService interface
    public class RouteService : IRouteService
    {
        // Repository object used to access route data from database
        private readonly IRouteRepository _routeRepository;

        // Constructor Dependency Injection
        // ASP.NET automatically provides the repository here
        public RouteService(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        // Get all routes and convert them into response DTOs
        public async Task<List<RouteResponseDto>> GetAllAsync()
        {
            // Get all route entities from repository
            var routes = await _routeRepository.GetAllAsync();

            // Convert each Route model into RouteResponseDto
            return routes.Select(r => new RouteResponseDto
            {
                RouteId = r.RouteId,
                RouteNumber = r.RouteNumber,
                RouteName = r.RouteName,
                StartLocation = r.StartLocation,
                EndLocation = r.EndLocation,
                TransportType = r.TransportType,
                IsActive = r.IsActive
            }).ToList();
        }

        // Get one route by ID and convert it into response DTO
        public async Task<RouteResponseDto?> GetByIdAsync(int id)
        {
            // Get route entity from repository
            var route = await _routeRepository.GetByIdAsync(id);

            // If route not found, return null
            if (route == null)
                return null;

            // Convert Route model into RouteResponseDto
            return new RouteResponseDto
            {
                RouteId = route.RouteId,
                RouteNumber = route.RouteNumber,
                RouteName = route.RouteName,
                StartLocation = route.StartLocation,
                EndLocation = route.EndLocation,
                TransportType = route.TransportType,
                IsActive = route.IsActive
            };
        }

        // Create a new route using CreateRouteDto
        public async Task<RouteResponseDto> CreateAsync(CreateRouteDto dto)
        {
            // Convert input DTO into Route model
            var route = new RouteModel
            {
                RouteNumber = dto.RouteNumber,
                RouteName = dto.RouteName,
                StartLocation = dto.StartLocation,
                EndLocation = dto.EndLocation,
                TransportType = dto.TransportType,

                // New routes are active by default
                IsActive = true
            };

            // Save route using repository
            var createdRoute = await _routeRepository.AddAsync(route);

            // Convert saved Route model into response DTO
            return new RouteResponseDto
            {
                RouteId = createdRoute.RouteId,
                RouteNumber = createdRoute.RouteNumber,
                RouteName = createdRoute.RouteName,
                StartLocation = createdRoute.StartLocation,
                EndLocation = createdRoute.EndLocation,
                TransportType = createdRoute.TransportType,
                IsActive = createdRoute.IsActive
            };
        }

        // Update an existing route using route ID and UpdateRouteDto
        public async Task<RouteResponseDto?> UpdateAsync(int id, UpdateRouteDto dto)
        {
            // Create Route model from incoming update DTO
            var route = new RouteModel
            {
                RouteId = id,
                RouteNumber = dto.RouteNumber,
                RouteName = dto.RouteName,
                StartLocation = dto.StartLocation,
                EndLocation = dto.EndLocation,
                TransportType = dto.TransportType,
                IsActive = dto.IsActive
            };

            // Send updated model to repository
            var updatedRoute = await _routeRepository.UpdateAsync(route);

            // If route not found in database, return null
            if (updatedRoute == null)
                return null;

            // Convert updated Route model into response DTO
            return new RouteResponseDto
            {
                RouteId = updatedRoute.RouteId,
                RouteNumber = updatedRoute.RouteNumber,
                RouteName = updatedRoute.RouteName,
                StartLocation = updatedRoute.StartLocation,
                EndLocation = updatedRoute.EndLocation,
                TransportType = updatedRoute.TransportType,
                IsActive = updatedRoute.IsActive
            };
        }

        // Delete route by ID
        public async Task<bool> DeleteAsync(int id)
        {
            // Call repository delete method and return result
            return await _routeRepository.DeleteAsync(id);
        }
    }
}