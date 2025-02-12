using Application.Core;
using Application.DTOs.Users;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Result<PagedResult<UserDto>>>> GetUsersAsync([FromBody] DefaultParams defaultParams)
    {
        var users = userRepository.GetAllUsersAsync(defaultParams);
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
    public async Task<ActionResult<Result<UserDetailDto>>> GetUserAsync([FromQuery] string userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound(Result<UserDetailDto>.FailureResult("User not found"));
        }
        var result = mapper.Map<UserDetailDto>(user);
        return Ok(Result<UserDetailDto>.SuccessResult(result));
    }

    [HttpPut("edit")]
    public async Task<ActionResult<Result<string>>> EditUserAsync(UserEditDto userEditDto)
    {
        var result = await userRepository.EditUserAsync(userEditDto);
        if (result == "User not found")
        {
            return NotFound(Result<string>.FailureResult(result));
        }
        return Ok(Result<string>.SuccessResult(null, result));
    }
}