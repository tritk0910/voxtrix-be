using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services;

public class MessageHub(IMessageRepository messageRepository) : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendChannelMessage(string authorId, string channelId, string content, List<IFormFile> attachments)
    {
        var result = await messageRepository.CreateChannelMessageAsync(authorId, channelId, content, attachments);

        if (!result.Success)
        {
            await Clients.User(authorId).SendAsync("UploadError", result.Message);
            return;
        }

        await Clients.Group(channelId).SendAsync("ReceiveChannelMessage", authorId, content);
    }

    public async Task SendDirectMessage(string authorId, string recipientId, string content, List<IFormFile> attachments)
    {
        var result = await messageRepository.CreateDirectMessageAsync(authorId, recipientId, content, attachments);

        if (!result.Success)
        {
            await Clients.User(authorId).SendAsync("UploadError", result.Message);
            return;
        }

        await Clients.User(recipientId).SendAsync("ReceiveDirectMessage", content);
    }

    public async Task EditMessageAsync(string messageId, string content)
    {
        var result = await messageRepository.EditMessageAsync(messageId, content);

        if (!result.Success)
        {
            await Clients.User(Context.UserIdentifier).SendAsync("EditError", result.Message);
            return;
        }

        await Clients.All.SendAsync("ReceiveEditMessage", messageId, content);
    }

    public async Task DeleteMessage(string messageId)
    {
        var result = await messageRepository.DeleteMessageAsync(messageId);

        if (!result.Success)
        {
            await Clients.User(Context.UserIdentifier).SendAsync("DeleteError", result.Message);
            return;
        }

        await Clients.All.SendAsync("ReceiveDeleteMessage", messageId);
    }
}