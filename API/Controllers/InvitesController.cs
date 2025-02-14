using System.Security.Claims;
using Application.Core;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
public class InvitesController(IInviteRepository inviteRepository) : ControllerBase
{
    [HttpGet]
    [Route("{inviteCode}")]
    public async Task<ActionResult<Result<bool>>> GetInvite(string inviteCode)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(Result<bool>.FailureResult("Unauthorized"));
        var result = await inviteRepository.GetInviteAsync(inviteCode, userId);
        if (result == "Invite not found") return BadRequest(Result<bool>.FailureResult("Invite not found"));
        if (result == "User is banned from the server") return BadRequest(Result<bool>.FailureResult("User is banned from the server"));
        if (result == "User is already a member of the server") return BadRequest(Result<bool>.FailureResult("User is already a member of the server"));
        if (result == "Invite has no remaining uses") return BadRequest(Result<bool>.FailureResult("Invite has no remaining uses"));
        return Ok(Result<bool>.SuccessResult(true, "Invite accepted"));
    }

    [HttpDelete]
    [Route("invites")]
    public async Task<ActionResult<Result<bool>>> DeleteInvite([FromQuery] string inviteId)
    {
        var result = await inviteRepository.DeleteInviteAsync(inviteId);
        if (result) return Ok(Result<bool>.SuccessResult(true, "Invite deleted"));

        return BadRequest(Result<bool>.FailureResult("Invite not found"));
    }
}