using System.Security.Claims;
using Application.Core;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public partial class UsersController
{
    [HttpPost("custom-status")]
    public async Task<ActionResult<Result<bool>>> SetCustomStatus([FromBody] string customStatus)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        if (user == null) return NotFound(Result<bool>.FailureResult("User not found"));

        user.CustomStatus = customStatus;
        await userManager.UpdateAsync(user);

        return Ok(Result<bool>.SuccessResult(true, "Custom status updated successfully"));
    }

    [HttpPost("status")]
    public async Task<ActionResult<Result<bool>>> SetStatus([FromBody] string status)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        if (user == null) return NotFound(Result<bool>.FailureResult("User not found"));

        if (!Enum.IsDefined(typeof(UserStatus), status))
        {
            return BadRequest(Result<bool>.FailureResult("Invalid status value"));
        }

        user.Status = Enum.Parse<UserStatus>(status);
        await userManager.UpdateAsync(user);

        return Ok(Result<bool>.SuccessResult(true, "Status updated successfully"));
    }
}