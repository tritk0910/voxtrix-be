using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class AccountsController(ITokenService tokenService, IMapper mapper, UserManager<AppUser> userManager) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> RegisterAsync(RegisterDto registerDto)
    {
        if (await userManager.Users.AnyAsync(x => x.UserName == registerDto.Username || x.Email == registerDto.Email))
        {
            return BadRequest("Username or email is already taken");
        }

        var user = mapper.Map<AppUser>(registerDto);
        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return CreateUserObject(user);
        }
        return BadRequest(result.Errors);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.UsernameOrEmail || x.UserName == loginDto.UsernameOrEmail);

        if (user == null) return Unauthorized("Invalid username or email");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (result)
        {
            return CreateUserObject(user);
        }

        return Unauthorized("Invalid password");
    }

    [HttpGet]
    public async Task<ActionResult<AppUser>> GetCurrentUser()
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        return user;
    }


    private UserDto CreateUserObject(AppUser user)
    {
        var userDto = mapper.Map<UserDto>(user);
        userDto.Token = tokenService.CreateToken(user);
        return userDto;
    }
}