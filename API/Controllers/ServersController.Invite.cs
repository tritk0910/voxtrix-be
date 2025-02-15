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
    public async Task<ActionResult<Result<InviteDto>>> UpdateInvite([FromBody] UpdateInviteDto updateInviteDto)
    {
        var result = await serverRepository.UpdateInviteAsync(updateInviteDto);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("invite/pause")]
    public async Task<ActionResult<Result<bool>>> PauseAllInvites([FromQuery] string serverId)
    {
        var result = await serverRepository.PauseInviteAsync(serverId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete]
    [Route("invite")]
    public async Task<ActionResult<Result<bool>>> DeleteInvite([FromQuery] string inviteId)
    {
        var result = await serverRepository.DeleteInviteAsync(inviteId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }
}
