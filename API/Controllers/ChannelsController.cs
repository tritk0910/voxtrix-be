using Application.Core;
using Application.DTOs.Channels;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ChannelsController(IChannelRepository channelRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Result<ChannelDto>>> GetChannelById([FromQuery] string channelId)
    {
        var result = await channelRepository.GetChannelByIdAsync(channelId);
        if (!result.Success) return NotFound(result.Message);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Result<ChannelDto>>> CreateChannel(CreateChannelDto createChannelDto)
    {
        var result = await channelRepository.CreateChannelAsync(createChannelDto);
        if (!result.Success) return BadRequest(result.Message);
        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult<Result<ChannelDto>>> UpdateChannel(UpdateChannelDto updateChannelDto)
    {
        var result = await channelRepository.UpdateChannelAsync(updateChannelDto);
        if (!result.Success) return NotFound(result.Message);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult<Result<bool>>> DeleteChannel([FromQuery] string channelId)
    {
        var result = await channelRepository.DeleteChannelAsync(channelId);
        if (!result.Success) return NotFound(result.Message);
        return Ok(result);
    }
}