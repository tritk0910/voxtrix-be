using System.Security.Claims;
using Application.Core;
using Application.DTOs.Servers;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Manages servers, including creation, retrieval, updating, and deletion of servers.
/// </summary>
[Authorize]
public partial class ServersController(IServerRepository serverRepository, IChannelRepository channelRepository) : BaseApiController
{
    /// <summary>
    /// Gets the list of servers the current user has joined.
    /// </summary>
    /// <returns>A list of servers the user has joined.</returns>
    [HttpGet("joined-servers")]
    public async Task<ActionResult<Result<List<ServerDto>>>> GetJoinedServersAsync()
    {
        var userId = GetCurrentUserId();
        var servers = await serverRepository.GetServersByUserId(userId);

        return Ok(Result<List<ServerDto>>.SuccessResult(servers));
    }

    /// <summary>
    /// Creates a new server.
    /// </summary>
    /// <param name="createServerDto">The details of the server to create.</param>
    /// <returns>The created server.</returns>
    [HttpPost]
    public async Task<ActionResult<Result<ServerDto>>> CreateServer([FromForm] CreateServerDto createServerDto)
    {
        var userId = GetCurrentUserId();
        var server = await serverRepository.CreateServer(createServerDto, userId);

        if (!server.Success) return BadRequest(server);

        return Ok(server);
    }

    /// <summary>
    /// Updates a server.
    /// </summary>
    /// <param name="editServerDto">The details of the server to update.</param>
    /// <returns>The updated server.</returns>
    [HttpPut]
    public async Task<ActionResult<Result<ServerDto>>> UpdateServer([FromForm] EditServerDto editServerDto)
    {
        var userId = GetCurrentUserId();
        var server = await serverRepository.UpdateServer(editServerDto, userId);

        if (!server.Success) return BadRequest(server);

        return Ok(server);
    }

    /// <summary>
    /// Gets basic information about a server.
    /// </summary>
    /// <param name="serverId">The ID of the server.</param>
    /// <returns>Basic information about the server.</returns>
    [HttpGet]
    public async Task<ActionResult<Result<ServerBasicDto>>> GetServerBasicAsync([FromQuery] string serverId)
    {
        var server = await serverRepository.GetServerBasicAsync(serverId);
        if (server != null) return Ok(Result<ServerBasicDto>.SuccessResult(server));

        return NotFound(Result<ServerBasicDto>.FailureResult("Server not found"));
    }

    /// <summary>
    /// Gets detailed information about a server.
    /// </summary>
    /// <param name="serverId">The ID of the server.</param>
    /// <returns>Detailed information about the server.</returns>
    [HttpGet("details")]
    public async Task<ActionResult<Result<ServerDetailsDto>>> GetServerDetailsAsync([FromQuery] string serverId)
    {
        var server = await serverRepository.GetServerDetailsAsync(serverId);
        if (server != null) return Ok(Result<ServerDetailsDto>.SuccessResult(server));

        return NotFound(Result<ServerDetailsDto>.FailureResult("Server not found"));
    }


    /// <summary>
    /// Gets the members of a server.
    /// </summary>
    /// <param name="serverId">The ID of the server.</param>
    /// <param name="defaultParams">The default parameters for pagination.</param>
    /// <returns>The members of the server.</returns>
    [HttpGet("members")]
    public async Task<ActionResult<Result<List<ServerMemberDto>>>> GetMembersByServerId([FromQuery] string serverId, [FromQuery] DefaultParams defaultParams)
    {
        var members = await serverRepository.GetMembersByServerId(serverId, defaultParams);
        var pagedMemberList = await PagedList<ServerMemberDto>.CreateAsync(members, defaultParams.PageNumber, defaultParams.PageSize);
        var result = new PagedResult<ServerMemberDto>
        {
            Items = pagedMemberList,
            CurrentPage = pagedMemberList.CurrentPage,
            TotalPages = pagedMemberList.TotalPages
        };

        return Ok(Result<PagedResult<ServerMemberDto>>.SuccessResult(result));
    }

    /// <summary>
    /// Deletes a server.
    /// </summary>
    /// <param name="serverId">The ID of the server to delete.</param>
    /// <returns>A result indicating whether the deletion was successful.</returns>
    [HttpDelete]
    public async Task<ActionResult<Result<bool>>> DeleteServer([FromQuery] string serverId)
    {
        var result = await serverRepository.DeleteServer(GetCurrentUserId(), serverId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Transfers ownership of a server to a new owner.
    /// </summary>
    /// <param name="serverId">The ID of the server.</param>
    /// <param name="newOwnerId">The ID of the new owner.</param>
    /// <returns>A result indicating whether the transfer was successful.</returns>
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

    /// <summary>
    /// Creates a server by running a script.
    /// </summary>
    /// <param name="createServerByScriptDto">The details of the server to create using a script.</param>
    /// <returns>The created server.</returns>
    [HttpPost("script")]
    public async Task<ActionResult<Result<ServerDto>>> CreateServerByScript([FromForm] CreateServerByScriptDto createServerByScriptDto)
    {
        var userId = GetCurrentUserId();
        var server = await serverRepository.CreateServerByScript(createServerByScriptDto, userId);

        if (!server.Success) return BadRequest(server);

        return Ok(server);
    }
}