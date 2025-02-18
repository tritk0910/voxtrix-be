using Microsoft.AspNetCore.SignalR;

namespace Application.Services;

public class AppHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendChannelMessage(string channelId, string user, string message)
    {
        await Clients.Group(channelId).SendAsync("ReceiveChannelMessage", user, message);
    }

    public async Task SendDirectMessage(string user, string message)
    {
        await Clients.User(user).SendAsync("ReceiveDirectMessage", message);
    }

    public async Task SendNotification(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
}