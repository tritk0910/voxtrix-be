using Application.Core;
using Application.DTOs.Servers.Channels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public partial class ServersController
{
    /// <summary>
    /// Retrieves a list of channels for a given server.
    /// </summary>
    /// <param name="serverId">The ID of the server.</param>
    /// <returns>A list of channels associated with the specified server.</returns>
    [HttpGet("channels")]
    public async Task<ActionResult<Result<List<ChannelDto>>>> GetChannelsByServerId(string serverId)
    {
        var result = await channelRepository.GetChannelsByServerIdAsync(serverId);
        if (result.Success == false) return BadRequest(result);
        return Ok(result);
    }
}
