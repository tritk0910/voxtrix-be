using Application.Core;
using Application.DTOs.Channels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public partial class ServersController
{
    [HttpGet("servers/{serverId}/channels")]
    public async Task<ActionResult<Result<List<ChannelDto>>>> GetChannelsByServerId(string serverId)
    {
        var result = await channelRepository.GetChannelsByServerIdAsync(serverId);
        return Ok(result);
    }
}
