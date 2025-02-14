using System.Security.Claims;
using Application.Core;
using Application.DTOs.Servers;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public partial class ServersController(IServerRepository serverRepository) : BaseApiController
{
    [HttpGet("joined-servers")]
    public async Task<ActionResult<Result<List<ServerDto>>>> GetJoinedServersAsync()
    {
        // Assuming you have a method to get the current user's ID
        var userId = GetCurrentUserId();

        // Assuming you have a service to get the servers the user has joined
        var servers = await serverRepository.GetServersByUserId(userId);

        return Ok(Result<List<ServerDto>>.SuccessResult(servers));
    }

    [HttpPost]
    public async Task<ActionResult<Result<ServerDto>>> CreateServer(CreateServerDto createServerDto)
    {
        // Assuming you have a method to get the current user's ID
        var userId = GetCurrentUserId();

        // Assuming you have a service to create a server
        var server = await serverRepository.CreateServer(createServerDto, userId);

        return Ok(Result<ServerDto>.SuccessResult(server));
    }

    [HttpGet]
    public async Task<ActionResult<Result<ServerBasicDto>>> GetServerBasicAsync([FromQuery] string serverId)
    {
        // Assuming you have a service to get the basic details of a server
        var server = await serverRepository.GetServerBasicAsync(serverId);

        if (server != null)
        {
            return Ok(Result<ServerBasicDto>.SuccessResult(server));
        }

        return NotFound(Result<ServerBasicDto>.FailureResult("Server not found"));
    }

    [HttpGet("details")]
    public async Task<ActionResult<Result<ServerDetailsDto>>> GetServerDetailsAsync([FromQuery] string serverId)
    {
        var server = await serverRepository.GetServerDetailsAsync(serverId);

        if (server != null)
        {
            return Ok(Result<ServerDetailsDto>.SuccessResult(server));
        }

        return NotFound(Result<ServerDetailsDto>.FailureResult("Server not found"));
    }

    [HttpDelete]
    public async Task<ActionResult<Result<bool>>> DeleteServer([FromQuery] string serverId)
    {
        // Assuming you have a service to delete a server
        var result = await serverRepository.DeleteServer(serverId);

        if (result)
        {
            return Ok(Result<bool>.SuccessResult(true, "Server deleted successfully"));
        }

        return NotFound(Result<bool>.FailureResult("Server not found"));
    }

    private string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}