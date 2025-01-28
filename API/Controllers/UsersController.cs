using Application.DTOs.Accounts;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync()
    {
        var users = await userRepository.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserDetailDto>> GetUserAsync(string username)
    {
        var user = await userRepository.GetUserByUsernameAsync(username);
        var result = mapper.Map<UserDetailDto>(user);
        return Ok(result);
    }
}