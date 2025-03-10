using System.Security.Claims;
using Application.Core;
using Application.DTOs.Users;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public partial class UsersController
{
    /// <summary>
    /// Sets a custom status for the authenticated user.
    /// </summary>
    /// <param name="editCustomStatusUserDto">The custom status to set.</param>
    /// <returns>A result containing the updated user details.</returns>
    /// <response code="200">Custom status updated successfully.</response>
    /// <response code="404">User not found.</response>
    [HttpPost("custom-status")]
    public async Task<ActionResult<Result<UserDetailsDto>>> SetCustomStatus([FromBody] EditCustomStatusUserDto editCustomStatusUserDto)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        if (user == null) return NotFound(Result<UserDetailsDto>.FailureResult("User not found"));

        user.CustomStatus = editCustomStatusUserDto.Status;
        await userManager.UpdateAsync(user);

        var userDetailsDto = mapper.Map<UserDetailsDto>(user);
        return Ok(Result<UserDetailsDto>.SuccessResult(userDetailsDto, "Custom status updated successfully"));
    }

    /// <summary>
    /// Sets a predefined status for the authenticated user.
    /// </summary>
    /// <param name="editStatusUserDto">The status to set.</param>
    /// <returns>A result containing the updated user details.</returns>
    /// <response code="200">Status updated successfully.</response>
    /// <response code="400">Invalid status value.</response>
    /// <response code="404">User not found.</response>
    [HttpPost("status")]
    public async Task<ActionResult<Result<UserDetailsDto>>> SetStatus([FromBody] EditStatusUserDto editStatusUserDto)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        if (user == null) return NotFound(Result<UserDetailsDto>.FailureResult("User not found"));

        if (!Enum.IsDefined(typeof(UserStatus), editStatusUserDto.Status))
        {
            return BadRequest(Result<UserDetailsDto>.FailureResult("Invalid status value"));
        }

        user.Status = editStatusUserDto.Status;
        await userManager.UpdateAsync(user);

        var userDetailsDto = mapper.Map<UserDetailsDto>(user);
        return Ok(Result<UserDetailsDto>.SuccessResult(userDetailsDto, "Status updated successfully"));
    }
}