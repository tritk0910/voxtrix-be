using Application.Core;
using Application.DTOs.Message;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IMessageRepository
{
    Task<IQueryable<DirectMessageDto>> GetConversationAsync(string userId1, string userId2, DefaultParams defaultParams);
    Task<IQueryable<MessageDto>> GetChannelMessagesAsync(string channelId, DefaultParams defaultParams);
    Task<Result<MessageDto>> CreateChannelMessageAsync(string authorId, string channelId, string content, List<IFormFile> attachments);
    Task<Result<DirectMessageDto>> CreateDirectMessageAsync(string authorId, string recipientId, string content, List<IFormFile> attachments);
    Task<Result<string>> EditMessageAsync(string messageId, string content);
    Task<Result<string>> DeleteMessageAsync(string messageId);
}