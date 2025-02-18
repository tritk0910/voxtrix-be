using System.Security.Claims;
using Application.Core;
using Application.DTOs.Accounts;
using Application.DTOs.Users;
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
    public async Task<ActionResult<Result<UserDto>>> RegisterAsync(RegisterDto registerDto)
    {
        if (await userManager.Users.AnyAsync(x => x.UserName == registerDto.Username || x.Email == registerDto.Email))
        {
            return BadRequest(Result<UserDto>.FailureResult("Username or email is already taken"));
        }

        var user = mapper.Map<AppUser>(registerDto);
        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return Ok(Result<UserDto>.SuccessResult(CreateUserObject(user), "User registered successfully"));
        }
        return BadRequest(Result<UserDto>.FailureResult("Registration failed"));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<Result<UserDto>>> Login(LoginDto loginDto)
    {
        var user = await userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == loginDto.UsernameOrEmail || x.UserName == loginDto.UsernameOrEmail);

        if (user == null) return Unauthorized(Result<UserDto>.FailureResult("Invalid username or email"));

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (result)
        {
            return Ok(Result<UserDto>.SuccessResult(CreateUserObject(user), "Login successful"));
        }

        return Unauthorized(Result<UserDto>.FailureResult("Invalid password"));
    }

    [HttpGet]
    public async Task<ActionResult<Result<AppUser>>> GetCurrentUser()
    {
        var user = await userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

        if (user == null) return NotFound(Result<AppUser>.FailureResult("User not found"));

        return Ok(Result<AppUser>.SuccessResult(user, "User retrieved successfully"));
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<ActionResult<Result<string>>> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user == null) return BadRequest(Result<string>.FailureResult("User not found"));

        // Generate OTP
        var otp = new Random().Next(100000, 999999).ToString();

        // Store OTP in Redis
        await redisService.SetOtpAsync(user.Id.ToString(), otp, TimeSpan.FromMinutes(10));

        // Send OTP to user's email
        var message = $"Your OTP for password reset is: {otp}";
        await emailService.SendEmailAsync(forgotPasswordDto.Email, "Password Reset OTP", message);

        return Ok(Result<string>.SuccessResult(null, "OTP sent successfully"));
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<ActionResult<Result<string>>> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null) return BadRequest(Result<string>.FailureResult("User not found"));

        // Retrieve OTP from Redis
        var storedOtp = await redisService.GetOtpAsync(user.Id.ToString());
        if (storedOtp != resetPasswordDto.Otp)
        {
            return BadRequest(Result<string>.FailureResult("Invalid or expired OTP"));
        }

        // Reset password
        var resetPassResult = await userManager.RemovePasswordAsync(user);
        if (!resetPassResult.Succeeded)
        {
            return BadRequest(Result<string>.FailureResult("Error removing old password"));
        }

        resetPassResult = await userManager.AddPasswordAsync(user, resetPasswordDto.NewPassword);
        if (!resetPassResult.Succeeded)
        {
            return BadRequest(Result<string>.FailureResult("Error setting new password"));
        }

        // Clear OTP from Redis
        await redisService.DeleteOtpAsync(user.Id.ToString());

        return Ok(Result<string>.SuccessResult(null, "Password has been reset successfully"));
    }

    private UserDto CreateUserObject(AppUser user)
    {
        var userDto = mapper.Map<UserDto>(user);
        var token = tokenService.CreateToken(user);
        Response.Headers.Append("Authorization", $"Bearer {token}");
        return userDto;
    }
}