using TransitPulse.API.DTOs;

namespace TransitPulse.API.Services
{
    public interface IRouteService
    {
        Task<List<RouteResponseDto>> GetAllAsync();
        Task<RouteResponseDto?> GetByIdAsync(int id);
        Task<RouteResponseDto> CreateAsync(CreateRouteDto dto);
        Task<RouteResponseDto?> UpdateAsync(int id, UpdateRouteDto dto);
        Task<bool> DeleteAsync(int id);
    }
}