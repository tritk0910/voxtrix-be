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
    public async Task<ActionResult<Result<bool>>> JoinServerViaInviteLink(string inviteCode)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(Result<bool>.FailureResult("Unauthorized"));
        var result = await inviteRepository.JoinServerViaInviteLinkAsync(inviteCode, userId);
        if (result == "User added to the server successfully") return Ok(Result<bool>.SuccessResult(true, "Invite accepted"));
        return BadRequest(Result<string>.FailureResult(result));
    }
}