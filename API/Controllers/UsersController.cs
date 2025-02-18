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

    [HttpPut]
    public async Task<ActionResult<Result<UserDetailsDto>>> EditUserAsync(UserEditDto userEditDto)
    {
        var result = await userRepository.EditUserAsync(userEditDto);
        if (!result.Success) return NotFound(result);

        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult<Result<UserDetailsDto>>> DeleteUserAsync([FromQuery] string userId)
    {
        var result = await userRepository.DeleteUserAsync(userId);
        if (!result.Success) return NotFound(result);

        return Ok(result);
    }
}