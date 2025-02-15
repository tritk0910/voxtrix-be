using System.Security.Claims;
using Application.Core;
using Application.DTOs.Users;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public partial class UsersController
{
    [HttpPost("custom-status")]
    public async Task<ActionResult<Result<UserDetailsDto>>> SetCustomStatus([FromBody] string customStatus)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        if (user == null) return NotFound(Result<UserDetailsDto>.FailureResult("User not found"));

        user.CustomStatus = customStatus;
        await userManager.UpdateAsync(user);

        var userDetailsDto = mapper.Map<UserDetailsDto>(user);
        return Ok(Result<UserDetailsDto>.SuccessResult(userDetailsDto, "Custom status updated successfully"));
    }

    [HttpPost("status")]
    public async Task<ActionResult<Result<UserDetailsDto>>> SetStatus([FromBody] string status)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        if (user == null) return NotFound(Result<UserDetailsDto>.FailureResult("User not found"));

        if (!Enum.IsDefined(typeof(UserStatus), status))
        {
            return BadRequest(Result<UserDetailsDto>.FailureResult("Invalid status value"));
        }

        user.Status = Enum.Parse<UserStatus>(status);
        await userManager.UpdateAsync(user);

        var userDetailsDto = mapper.Map<UserDetailsDto>(user);
        return Ok(Result<UserDetailsDto>.SuccessResult(userDetailsDto, "Status updated successfully"));
    }
}