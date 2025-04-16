using Application.Core;
using Application.DTOs.Message;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services.SignalR;

public class MessageRepository(DataContext context, IMapper mapper, ICloudinaryService cloudinaryService) : IMessageRepository
{
    public Task<IQueryable<DirectMessageDto>> GetConversationAsync(string user1Id, string user2Id, DefaultParams defaultParams)
    {
        var result = context.Messages
            .Where(dm => (dm.AuthorId == user1Id && dm.RecipientId == user2Id) ||
                    (dm.AuthorId == user2Id && dm.RecipientId == user1Id))
            .AsNoTracking()
            .OrderBy(dm => dm.CreatedAt)
            .ProjectTo<DirectMessageDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        if (!string.IsNullOrEmpty(defaultParams.Search))
        {
            result = result.Where(dm => dm.Content.Contains(defaultParams.Search));
        }

        result = defaultParams.SortBy switch
        {
            _ => defaultParams.OrderBy == "ASC" ? result.OrderBy(dm => dm.CreatedAt) : result.OrderByDescending(dm => dm.CreatedAt),
        };
        return Task.FromResult(result);
    }

    public Task<IQueryable<MessageDto>> GetChannelMessagesAsync(string channelId, DefaultParams defaultParams)
    {
        var result = context.Messages
            .Where(m => m.ChannelId == channelId)
            .AsNoTracking()
            .OrderBy(m => m.CreatedAt)
            .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        if (!string.IsNullOrEmpty(defaultParams.Search))
        {
            result = result.Where(m => m.Content.Contains(defaultParams.Search));
        }

        result = defaultParams.SortBy switch
        {
            _ => defaultParams.OrderBy == "ASC" ? result.OrderBy(m => m.CreatedAt) : result.OrderByDescending(m => m.CreatedAt),
        };
        return Task.FromResult(result);
    }

    public async Task<Result<MessageDto>> CreateChannelMessageAsync(string authorId, string channelId, string content, List<IFormFile> attachments)
    {
        var author = await context.Users.FindAsync(authorId);
        if (author == null)
            return Result<MessageDto>.FailureResult("Author not found");

        var newMessage = new Message
        {
            ChannelId = channelId,
            AuthorId = authorId,
            Author = author,
            Content = content,
        };

        if (attachments != null && attachments.Count > 0)
        {
            newMessage.AttachmentURLs = [];
            foreach (var attachment in attachments)
            {
                var attachmentURL = await cloudinaryService.UploadImageAsync(attachment);

                if (attachmentURL.Error != null)
                    return Result<MessageDto>.FailureResult(attachmentURL.Error.Message);

                newMessage.AttachmentURLs.Add(attachmentURL.SecureUrl.AbsoluteUri);
            }
        }

        context.Messages.Add(newMessage);
        var result = await context.SaveChangesAsync() > 0;

        if (!result)
            return Result<MessageDto>.FailureResult("Failed to create channel message");

        var messageDto = mapper.Map<MessageDto>(newMessage);
        return Result<MessageDto>.SuccessResult(messageDto, "Channel message created successfully");
    }

    public async Task<Result<DirectMessageDto>> CreateDirectMessageAsync(string authorId, string recipientId, string content, List<IFormFile> attachments)
    {
        var newMessage = new Message
        {
            AuthorId = authorId,
            RecipientId = recipientId,
            Content = content,
        };

        if (attachments.Count > 0)
        {
            newMessage.AttachmentURLs = [];
            foreach (var attachment in attachments)
            {
                var attachmentURL = await cloudinaryService.UploadImageAsync(attachment);

                if (attachmentURL.Error != null)
                    return Result<DirectMessageDto>.FailureResult(attachmentURL.Error.Message);

                newMessage.AttachmentURLs.Add(attachmentURL.SecureUrl.AbsoluteUri);
            }
        }

        context.Messages.Add(newMessage);
        var result = await context.SaveChangesAsync() > 0;

        if (!result)
            return Result<DirectMessageDto>.FailureResult("Failed to create direct message");

        var messageDto = mapper.Map<DirectMessageDto>(newMessage);
        return Result<DirectMessageDto>.SuccessResult(messageDto, "Direct message created successfully");
    }

    public async Task<Result<string>> EditMessageAsync(string messageId, string content)
    {
        var message = await context.Messages.FindAsync(messageId);

        if (message == null)
            return Result<string>.FailureResult("Message not found");

        message.Content = content;
        message.EditedAt = DateTime.UtcNow;

        var result = await context.SaveChangesAsync() > 0;

        if (!result)
            return Result<string>.FailureResult("Failed to edit message");

        return Result<string>.SuccessResult(messageId, "Message edited successfully");
    }

    public async Task<Result<string>> DeleteMessageAsync(string messageId)
    {
        var message = await context.Messages.FindAsync(messageId);

        if (message == null)
            return Result<string>.FailureResult("Message not found");

        context.Messages.Remove(message);
        var result = await context.SaveChangesAsync() > 0;

        if (!result)
            return Result<string>.FailureResult("Failed to delete message");

        return Result<string>.SuccessResult(messageId, "Message deleted successfully");
    }
}