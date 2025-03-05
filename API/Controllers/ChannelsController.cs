using Application.Core;
using Application.DTOs.Channels;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Manages channels, including creation, retrieval, updating, and deletion of channels.
/// </summary>
[Authorize]
public class ChannelsController(IChannelRepository channelRepository) : BaseApiController
{
    /// <summary>
    /// Retrieves a channel by its ID.
    /// </summary>
    /// <param name="channelId">The ID of the channel to retrieve.</param>
    /// <returns>A channel object if found, otherwise a not found response.</returns>
    [HttpGet]
    public async Task<ActionResult<Result<ChannelDto>>> GetChannelById([FromQuery] string channelId)
    {
        var result = await channelRepository.GetChannelByIdAsync(channelId);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new channel.
    /// </summary>
    /// <param name="createChannelDto">The data transfer object containing the details of the channel to create.</param>
    /// <returns>The created channel object.</returns>
    [HttpPost]
    public async Task<ActionResult<Result<ChannelDto>>> CreateChannel(CreateChannelDto createChannelDto)
    {
        var result = await channelRepository.CreateChannelAsync(createChannelDto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing channel.
    /// </summary>
    /// <param name="updateChannelDto">The data transfer object containing the updated details of the channel.</param>
    /// <returns>The updated channel object if successful, otherwise a not found response.</returns>
    [HttpPut]
    public async Task<ActionResult<Result<ChannelDto>>> UpdateChannel(UpdateChannelDto updateChannelDto)
    {
        var result = await channelRepository.UpdateChannelAsync(updateChannelDto);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a channel by its ID.
    /// </summary>
    /// <param name="channelId">The ID of the channel to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    [HttpDelete]
    public async Task<ActionResult<Result<bool>>> DeleteChannel([FromQuery] string channelId)
    {
        var result = await channelRepository.DeleteChannelAsync(channelId);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }
}