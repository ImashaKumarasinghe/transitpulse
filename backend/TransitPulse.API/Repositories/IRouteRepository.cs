using TransitPulse.API.Models;
using Route = TransitPulse.API.Models.Route;

namespace TransitPulse.API.Repositories
{
    public interface IRouteRepository
    {
        Task<List<Route>> GetAllAsync();
        Task<Route?> GetByIdAsync(int id);
        Task<Route?> AddAsync(Route route);
        Task<Route?> UpdateAsync(Route route);
        Task<bool> DeleteAsync(int id);
    }
}