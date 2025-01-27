using System.Security.Claims;
using Application.DTOs.Accounts;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class AccountsController(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, IEmailService emailService, IRedisService redisService) : BaseApiController
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

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user == null) return BadRequest("User not found");

        // Generate OTP
        var otp = new Random().Next(100000, 999999).ToString();

        // Store OTP in Redis
        await redisService.SetOtpAsync(user.Id.ToString(), otp, TimeSpan.FromMinutes(10));

        // Send OTP to user's email
        var message = $"Your OTP for password reset is: {otp}";
        await emailService.SendEmailAsync(forgotPasswordDto.Email, "Password Reset OTP", message);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null) return BadRequest("User not found");

        // Retrieve OTP from Redis
        var storedOtp = await redisService.GetOtpAsync(user.Id.ToString());
        if (storedOtp != resetPasswordDto.Otp)
        {
            return BadRequest("Invalid or expired OTP");
        }

        // Reset password
        var resetPassResult = await userManager.RemovePasswordAsync(user);
        if (!resetPassResult.Succeeded)
        {
            return BadRequest("Error removing old password");
        }

        resetPassResult = await userManager.AddPasswordAsync(user, resetPasswordDto.NewPassword);
        if (!resetPassResult.Succeeded)
        {
            return BadRequest("Error setting new password");
        }

        // Clear OTP from Redis
        await redisService.DeleteOtpAsync(user.Id.ToString());

        return Ok("Password has been reset successfully");
    }
}