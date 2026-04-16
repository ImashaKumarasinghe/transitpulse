using Microsoft.AspNetCore.SignalR; // SignalR library

namespace TransitPulse.API.Hubs
{
    // Hub class used for real-time communication
    public class RouteHub : Hub
    {
        // Method for a user to join a specific route group
        public async Task JoinRouteGroup(string routeGroupName)
        {
            // Add the current user connection to a group
            // Context.ConnectionId = unique ID for each connected client
            await Groups.AddToGroupAsync(Context.ConnectionId, routeGroupName);
        }

        // Method for a user to leave a route group
        public async Task LeaveRouteGroup(string routeGroupName)
        {
            // Remove the user from the group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, routeGroupName);
        }
    }
}