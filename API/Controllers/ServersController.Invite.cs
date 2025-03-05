using Application.Core;
using Application.DTOs.Invites;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public partial class ServersController
{
    /// <summary>
    /// Creates a new invite.
    /// </summary>
    /// <param name="createInviteDto">The invite details.</param>
    /// <returns>The created invite.</returns>
    [HttpPost("invite")]
    public async Task<ActionResult<Result<InviteDto>>> CreateInvite([FromBody] CreateInviteDto createInviteDto)
    {
        var invite = await serverRepository.CreateInvite(createInviteDto);
        return Ok(Result<InviteDto>.SuccessResult(invite));
    }

    /// <summary>
    /// Updates an existing invite.
    /// </summary>
    /// <param name="updateInviteDto">The updated invite details.</param>
    /// <returns>The updated invite.</returns>
    [HttpPut("invite")]
    public async Task<ActionResult<Result<InviteDto>>> UpdateInvite([FromBody] UpdateInviteDto updateInviteDto)
    {
        var result = await serverRepository.UpdateInviteAsync(updateInviteDto);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Pauses all invites for a server.
    /// </summary>
    /// <param name="serverId">The ID of the server.</param>
    /// <returns>A boolean indicating success.</returns>
    [HttpPost("invite/pause")]
    public async Task<ActionResult<Result<bool>>> PauseAllInvites([FromQuery] string serverId)
    {
        var result = await serverRepository.PauseInviteAsync(serverId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes an invite.
    /// </summary>
    /// <param name="inviteId">The ID of the invite to delete.</param>
    /// <returns>A boolean indicating success.</returns>
    [HttpDelete("invite")]
    public async Task<ActionResult<Result<bool>>> DeleteInvite([FromQuery] string inviteId)
    {
        var result = await serverRepository.DeleteInviteAsync(inviteId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }
}
