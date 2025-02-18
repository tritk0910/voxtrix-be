using System.Security.Claims;
using Application.Core;
using Application.DTOs.Servers;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public partial class ServersController(IServerRepository serverRepository, IChannelRepository channelRepository) : BaseApiController
{
    [HttpGet("joined-servers")]
    public async Task<ActionResult<Result<List<ServerDto>>>> GetJoinedServersAsync()
    {
        var userId = GetCurrentUserId();
        var servers = await serverRepository.GetServersByUserId(userId);

        return Ok(Result<List<ServerDto>>.SuccessResult(servers));
    }

    [HttpPost]
    public async Task<ActionResult<Result<ServerDto>>> CreateServer(CreateServerDto createServerDto)
    {
        var userId = GetCurrentUserId();
        var server = await serverRepository.CreateServer(createServerDto, userId);

        return Ok(Result<ServerDto>.SuccessResult(server));
    }

    [HttpGet]
    public async Task<ActionResult<Result<ServerBasicDto>>> GetServerBasicAsync([FromQuery] string serverId)
    {
        var server = await serverRepository.GetServerBasicAsync(serverId);
        if (server != null) return Ok(Result<ServerBasicDto>.SuccessResult(server));

        return NotFound(Result<ServerBasicDto>.FailureResult("Server not found"));
    }

    [HttpGet("details")]
    public async Task<ActionResult<Result<ServerDetailsDto>>> GetServerDetailsAsync([FromQuery] string serverId)
    {
        var server = await serverRepository.GetServerDetailsAsync(serverId);
        if (server != null) return Ok(Result<ServerDetailsDto>.SuccessResult(server));

        return NotFound(Result<ServerDetailsDto>.FailureResult("Server not found"));
    }

    [HttpDelete]
    public async Task<ActionResult<Result<bool>>> DeleteServer([FromQuery] string serverId)
    {
        var result = await serverRepository.DeleteServer(GetCurrentUserId(), serverId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("ownership")]
    public async Task<ActionResult<Result<ServerTransferDto>>> TransferOwnership([FromQuery] string serverId, [FromQuery] string newOwnerId)
    {
        var result = await serverRepository.TransferOwnership(GetCurrentUserId(), serverId, newOwnerId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}