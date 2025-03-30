using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services.SignalR;

public partial class MessageHub : Hub
{
#nullable enable
    public async Task SendChannelMessage(string channelId, string content, List<IFormFile>? attachments)
    {
        var authorId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await messageRepository.CreateChannelMessageAsync(authorId, channelId, content, attachments);

        if (!result.Success)
        {
            await Clients.User(authorId).SendAsync("UploadError", result.Message);
            return;
        }

        await Clients.Group(channelId).SendAsync("ReceiveChannelMessage", result.Data);
    }

    public async Task EditChannelMessage(string channelId, string messageId, string content)
    {
        var result = await messageRepository.EditMessageAsync(messageId, content);

        if (!result.Success)
        {
            await Clients.User(Context.UserIdentifier).SendAsync("EditError", result.Message);
            return;
        }

        await Clients.Group(channelId).SendAsync("ReceiveEditMessage", messageId, content);
    }

    public async Task JoinChannel(string channelId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, channelId);
    }

    public async Task LeaveChannel(string channelId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelId);
    }
}