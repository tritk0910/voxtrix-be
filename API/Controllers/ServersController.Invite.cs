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
}
