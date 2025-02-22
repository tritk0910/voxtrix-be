using Application.Core;
using Application.DTOs.Users;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public partial class UsersController(UserManager<AppUser> userManager, IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    /// <summary>
    /// Retrieves a paginated list of users.
    /// </summary>
    /// <param name="defaultParams">Pagination parameters.</param>
    /// <returns>A paginated list of users.</returns>
    [HttpGet]
    public async Task<ActionResult<Result<PagedResult<UserDto>>>> GetUsers([FromBody] DefaultParams defaultParams)
    {
        var users = await userRepository.GetAllUsersAsync(defaultParams);
        var pagedUsers = await PagedList<UserDto>.CreateAsync(users, defaultParams.PageNumber, defaultParams.PageSize);

        var result = new PagedResult<UserDto>
        {
            Items = pagedUsers,
            CurrentPage = pagedUsers.CurrentPage,
            TotalPages = pagedUsers.TotalPages
        };

        return Ok(Result<PagedResult<UserDto>>.SuccessResult(result));
    }

    /// <summary>
    /// Retrieves details of a specific user by ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>User details.</returns>
    [HttpGet("details")]
    public async Task<ActionResult<Result<UserDetailsDto>>> GetUserAsync([FromQuery] string userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound(Result<UserDetailsDto>.FailureResult("User not found"));
        }
        var result = mapper.Map<UserDetailsDto>(user);
        return Ok(Result<UserDetailsDto>.SuccessResult(result));
    }

    /// <summary>
    /// Edits a user's details.
    /// </summary>
    /// <param name="userEditDto">The user details to edit.</param>
    /// <returns>The updated user details.</returns>
    [HttpPut]
    public async Task<ActionResult<Result<UserDetailsDto>>> EditUserAsync(UserEditDto userEditDto)
    {
        var result = await userRepository.EditUserAsync(userEditDto);
        if (!result.Success) return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>The result of the deletion.</returns>
    [HttpDelete]
    public async Task<ActionResult<Result<UserDetailsDto>>> DeleteUserAsync([FromQuery] string userId)
    {
        var result = await userRepository.DeleteUserAsync(userId);
        if (!result.Success) return NotFound(result);

        return Ok(result);
    }
}