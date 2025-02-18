using Application.Core;
using Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public partial class UsersController
{
    [HttpGet("blocked")]
    public async Task<ActionResult<Result<PagedResult<BlockedUserDto>>>> GetBlockedUsersAsync([FromQuery] string userId, [FromBody] DefaultParams defaultParams)
    {
        var blockedUsers = await userRepository.GetBlockedUsersAsync(userId, defaultParams);
        if (blockedUsers == null) return NotFound(Result<PagedResult<BlockedUserDto>>.FailureResult("User not found"));

        var pagedBlockedUsers = await PagedList<BlockedUserDto>.CreateAsync(blockedUsers, defaultParams.PageNumber, defaultParams.PageSize);

        var result = new PagedResult<BlockedUserDto>
        {
            Items = pagedBlockedUsers,
            CurrentPage = pagedBlockedUsers.CurrentPage,
            TotalPages = pagedBlockedUsers.TotalPages
        };

        return Ok(Result<PagedResult<BlockedUserDto>>.SuccessResult(result));
    }

    [HttpPost("block")]
    public async Task<ActionResult<Result<BlockedUserDto>>> BlockUserAsync([FromQuery] string userId, [FromQuery] string targetId)
    {
        var result = await userRepository.BlockUserAsync(userId, targetId);
        if (!result.Success) return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("block")]
    public async Task<ActionResult<Result<bool>>> UnblockUserAsync([FromQuery] string blockId)
    {
        var result = await userRepository.UnblockUserAsync(blockId);
        if (!result.Success) return NotFound(result);

        return Ok(result);
    }
}