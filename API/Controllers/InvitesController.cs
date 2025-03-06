using System.Security.Claims;
using Application.Core;
using Application.DTOs.Invites;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Manages server invites, including joining a server via an invite link.
/// </summary>
[ApiController]
[Authorize]
public class InvitesController(IInviteRepository inviteRepository) : BaseApiController
{
    /// <summary>
    /// Joins a server using an invite link.
    /// </summary>
    /// <param name="inviteCode">The invite code to join the server.</param>
    /// <returns>A result indicating whether the operation was successful.</returns>
    [HttpGet]
    public async Task<ActionResult<Result<bool>>> JoinServerViaInviteLink(string inviteCode)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(Result<bool>.FailureResult("Unauthorized"));
        var result = await inviteRepository.JoinServerViaInviteLinkAsync(inviteCode, userId);
        if (result == "User added to the server successfully") return Ok(Result<bool>.SuccessResult(true, "Invite accepted"));
        return BadRequest(Result<string>.FailureResult(result));
    }

    /// <summary>
    /// Creates a new invite.
    /// </summary>
    /// <param name="createInviteDto">The invite details.</param>
    /// <returns>The created invite.</returns>
    [HttpPost]
    public async Task<ActionResult<Result<InviteDto>>> CreateInvite([FromBody] CreateInviteDto createInviteDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var invite = await inviteRepository.CreateInvite(createInviteDto, userId);
        return Ok(Result<InviteDto>.SuccessResult(invite));
    }

    /// <summary>
    /// Updates an existing invite.
    /// </summary>
    /// <param name="updateInviteDto">The updated invite details.</param>
    /// <returns>The updated invite.</returns>
    [HttpPut]
    public async Task<ActionResult<Result<InviteDto>>> UpdateInvite([FromBody] UpdateInviteDto updateInviteDto)
    {
        var result = await inviteRepository.UpdateInviteAsync(updateInviteDto);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Pauses all invites for a server.
    /// </summary>
    /// <param name="serverId">The ID of the server.</param>
    /// <returns>A boolean indicating success.</returns>
    [HttpPost("pause")]
    public async Task<ActionResult<Result<bool>>> PauseAllInvites([FromQuery] string serverId)
    {
        var result = await inviteRepository.PauseInviteAsync(serverId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes an invite.
    /// </summary>
    /// <param name="inviteId">The ID of the invite to delete.</param>
    /// <returns>A boolean indicating success.</returns>
    [HttpDelete]
    public async Task<ActionResult<Result<bool>>> DeleteInvite([FromQuery] string inviteId)
    {
        var result = await inviteRepository.DeleteInviteAsync(inviteId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }
}