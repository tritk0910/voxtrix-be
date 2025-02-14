using Application.Core;
using Application.DTOs.Invites;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public partial class ServersController
{
    [HttpPost("invite")]
    public async Task<ActionResult<Result<InviteDto>>> CreateInvite([FromBody] CreateInviteDto createInviteDto)
    {
        var invite = await serverRepository.CreateInvite(createInviteDto);
        return Ok(Result<InviteDto>.SuccessResult(invite));
    }

    [HttpPut("invite")]
    public async Task<ActionResult<Result<bool>>> UpdateInvite([FromBody] UpdateInviteDto updateInviteDto)
    {
        var result = await serverRepository.UpdateInviteAsync(updateInviteDto);
        if (result) return Ok(Result<bool>.SuccessResult(true, "Invite updated"));

        return BadRequest(Result<bool>.FailureResult("Invite not found"));
    }

    [HttpPost("invite/pause")]
    public async Task<ActionResult<Result<string>>> PauseAllInvites([FromQuery] string serverId)
    {
        var result = await serverRepository.PauseInviteAsync(serverId);
        if (result == "Invites paused") return Ok(Result<bool>.SuccessResult(true, result));

        return BadRequest(Result<bool>.FailureResult(result));
    }

    [HttpDelete]
    [Route("invite")]
    public async Task<ActionResult<Result<bool>>> DeleteInvite([FromQuery] string inviteId)
    {
        var result = await serverRepository.DeleteInviteAsync(inviteId);
        if (result) return Ok(Result<bool>.SuccessResult(true, "Invite deleted"));

        return BadRequest(Result<bool>.FailureResult("Invite not found"));
    }
}
