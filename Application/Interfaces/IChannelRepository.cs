using Application.DTOs.Channels;
using Application.Core;

namespace Application.Interfaces;

public interface IChannelRepository
{
    Task<Result<ChannelDto>> GetChannelByIdAsync(string channelId);
    Task<Result<List<ChannelDto>>> GetChannelsByServerIdAsync(string serverId);
    Task<Result<ChannelDto>> CreateChannelAsync(CreateChannelDto createChannelDto);
    Task<Result<ChannelDto>> UpdateChannelAsync(UpdateChannelDto updateChannelDto);
    Task<Result<bool>> DeleteChannelAsync(string channelId);
}