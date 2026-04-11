using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransitPulse.API.DTOs;
using TransitPulse.API.Services;

namespace TransitPulse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoutes()
        {
            var routes = await _routeService.GetAllAsync();
            return Ok(routes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRouteById(int id)
        {
            var route = await _routeService.GetByIdAsync(id);

            if (route == null)
                return NotFound("Route not found");

            return Ok(route);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRoute(CreateRouteDto dto)
        {
            var createdRoute = await _routeService.CreateAsync(dto);
            return Ok(createdRoute);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(int id, UpdateRouteDto dto)
        {
            var updatedRoute = await _routeService.UpdateAsync(id, dto);

            if (updatedRoute == null)
                return NotFound("Route not found");

            return Ok(updatedRoute);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            var deleted = await _routeService.DeleteAsync(id);

            if (!deleted)
                return NotFound("Route not found");

            return Ok("Route deleted successfully");
        }
    }
}