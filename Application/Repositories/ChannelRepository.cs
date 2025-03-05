using Application.DTOs.Channels;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Core;

namespace Application.Repositories;

public class ChannelRepository(DataContext context, IMapper mapper) : IChannelRepository
{
    public async Task<Result<ChannelDto>> CreateChannelAsync(CreateChannelDto createChannelDto)
    {
        var parentChannel = await context.Channels.FindAsync(createChannelDto.ParentChannelId);
        if (createChannelDto.ParentChannelId != null && parentChannel == null) return Result<ChannelDto>.FailureResult("Parent channel not found");
        if (parentChannel != null && parentChannel.ChannelType != ChannelType.Category) return Result<ChannelDto>.FailureResult("Parent channel must be a category");

        if (createChannelDto.Type == ChannelType.Category && createChannelDto.ParentChannelId != null)
        {
            return Result<ChannelDto>.FailureResult("Category channels cannot have a parent channel");
        }

        var channel = mapper.Map<Channel>(createChannelDto);
        context.Channels.Add(channel);
        var saveResult = await context.SaveChangesAsync() > 0;
        if (!saveResult) return Result<ChannelDto>.FailureResult("Failed to create channel");

        var channelDto = mapper.Map<ChannelDto>(channel);
        return Result<ChannelDto>.SuccessResult(channelDto, "Channel created successfully");
    }

    public async Task<Result<bool>> DeleteChannelAsync(string channelId)
    {
        var channel = await context.Channels.FindAsync(channelId);
        if (channel == null) return Result<bool>.FailureResult("Channel not found");

        context.Channels.Remove(channel);
        var saveResult = await context.SaveChangesAsync() > 0;
        if (!saveResult) return Result<bool>.FailureResult("Failed to delete channel");

        return Result<bool>.SuccessResult(true, "Channel deleted successfully");
    }

    public async Task<Result<ChannelDto>> GetChannelByIdAsync(string channelId)
    {
        var channel = await context.Channels
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ChannelId == channelId);

        if (channel == null) return Result<ChannelDto>.FailureResult("Channel not found");

        var channelDto = mapper.Map<ChannelDto>(channel);
        return Result<ChannelDto>.SuccessResult(channelDto);
    }

    public async Task<Result<List<ChannelDto>>> GetChannelsByServerIdAsync(string serverId)
    {
        var channels = await context.Channels
            .Where(c => c.ServerId == serverId)
            .AsNoTracking()
            .ProjectTo<ChannelDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return Result<List<ChannelDto>>.SuccessResult(channels);
    }

    public async Task<Result<ChannelDto>> UpdateChannelAsync(UpdateChannelDto updateChannelDto)
    {
        var channel = await context.Channels.FindAsync(updateChannelDto.ChannelId);
        if (channel == null) return Result<ChannelDto>.FailureResult("Channel not found");

        if (updateChannelDto.ParentChannelId != null)
        {
            var parentChannel = await context.Channels.FindAsync(updateChannelDto.ParentChannelId);
            if (parentChannel != null && parentChannel.ChannelType != ChannelType.Category)
                return Result<ChannelDto>.FailureResult("Parent channel must be a category");

            channel.ParentChannelId = updateChannelDto.ParentChannelId;
        }

        mapper.Map(updateChannelDto, channel);
        var saveResult = await context.SaveChangesAsync() > 0;
        if (!saveResult) return Result<ChannelDto>.FailureResult("Failed to update channel");

        var channelDto = mapper.Map<ChannelDto>(channel);
        return Result<ChannelDto>.SuccessResult(channelDto, "Channel updated successfully");
    }
}