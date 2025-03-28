using System.Security.Claims;
using System.Threading.Tasks;
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

/// <summary>
/// Manages user accounts, including registration, login, password reset, and OTP verification.
/// </summary>
[Authorize]
public class AccountsController(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, IEmailService emailService, IRedisService redisService, IAccountRepository accountRepository) : BaseApiController
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerDto">The registration details.</param>
    /// <returns>A result indicating success or failure.</returns>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<Result<bool>>> RegisterAsync(RegisterDto registerDto)
    {
        if (await userManager.Users.AnyAsync(x => x.UserName == registerDto.Username || x.Email == registerDto.Email))
        {
            return BadRequest(Result<UserDto>.FailureResult("Username or email is already taken"));
        }

        var user = mapper.Map<AppUser>(registerDto);
        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return Ok(Result<bool>.SuccessResult(await CreateUserObject(user), "User registered successfully"));
        }
        return BadRequest(Result<UserDto>.FailureResult("Registration failed"));
    }

    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="loginDto">The login details.</param>
    /// <returns>A result indicating success or failure.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<Result<bool>>> Login(LoginDto loginDto)
    {
        var user = await userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == loginDto.UsernameOrEmail || x.UserName == loginDto.UsernameOrEmail);

        if (user == null) return Unauthorized(Result<UserDto>.FailureResult("Invalid username or email"));

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (result)
        {
            return Ok(Result<bool>.SuccessResult(await CreateUserObject(user), "Login successful"));
        }

        return Unauthorized(Result<UserDto>.FailureResult("Invalid password"));
    }

    /// <summary>
    /// Gets the current logged-in user.
    /// </summary>
    /// <returns>The current user.</returns>
    [HttpGet]
    public async Task<ActionResult<Result<AppUser>>> GetCurrentUser()
    {
        var user = await userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
        var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        var isValidToken = tokenService.ValidateToken(token);
        if (!isValidToken) return Unauthorized(Result<AppUser>.FailureResult("Invalid token"));
        if (user == null) return NotFound(Result<AppUser>.FailureResult("User not found"));

        return Ok(Result<AppUser>.SuccessResult(user, "User retrieved successfully"));
    }

    /// <summary>
    /// Initiates the password reset process by sending an OTP to the user's email.
    /// </summary>
    /// <param name="forgotPasswordDto">The email of the user who forgot their password.</param>
    /// <returns>A result indicating success or failure.</returns>
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

    /// <summary>
    /// Verifies the OTP sent to the user's email.
    /// </summary>
    /// <param name="verifyOtpDto">The OTP verification details.</param>
    /// <returns>A result indicating success or failure.</returns>
    [AllowAnonymous]
    [HttpPost("verify-otp")]
    public async Task<ActionResult<Result<string>>> VerifyOtp(VerifyOtpDto verifyOtpDto)
    {
        var user = await userManager.FindByEmailAsync(verifyOtpDto.Email);
        if (user == null) return BadRequest(Result<string>.FailureResult("User not found"));

        // Retrieve OTP from Redis
        var storedOtp = await redisService.GetOtpAsync(user.Id.ToString());
        if (storedOtp != verifyOtpDto.Otp)
        {
            return BadRequest(Result<string>.FailureResult("Invalid or expired OTP"));
        }

        // Set OTP verified flag in Redis
        await redisService.SetOtpVerifiedAsync(user.Id.ToString(), TimeSpan.FromMinutes(10));

        return Ok(Result<string>.SuccessResult(null, "OTP verified successfully"));
    }

    /// <summary>
    /// Resets the user's password after OTP verification.
    /// </summary>
    /// <param name="resetPasswordDto">The password reset details.</param>
    /// <returns>A result indicating success or failure.</returns>
    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<ActionResult<Result<string>>> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null) return BadRequest(Result<string>.FailureResult("User not found"));

        // Check OTP verified flag in Redis
        var isOtpVerified = await redisService.IsOtpVerifiedAsync(user.Id.ToString());
        if (!isOtpVerified)
        {
            return BadRequest(Result<string>.FailureResult("OTP not verified"));
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

        // Clear OTP and verified flag from Redis
        await redisService.DeleteOtpAsync(user.Id.ToString());
        await redisService.DeleteOtpVerifiedAsync(user.Id.ToString());

        return Ok(Result<string>.SuccessResult(null, "Password has been reset successfully"));
    }

    /// <summary>
    /// Refreshes the user's JWT token.
    /// </summary>
    /// <param name="refreshToken">The current refresh token.</param>
    /// <returns>A new access token and refresh token attached to header of the response</returns>
    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<ActionResult<Result<RefreshTokenCookieResponse>>> RefreshTokenAsync(string refreshToken)
    {
        var newToken = await tokenService.RefreshTokenAsync(refreshToken);
        if (newToken == null) return Unauthorized(Result<RefreshTokenCookieResponse>.FailureResult("Invalid refresh token"));
        AssignTokensToResponseHeaderAndCookie(newToken.Token, newToken);
        return Ok(Result<RefreshTokenCookieResponse>.SuccessResult(null, "Token refreshed successfully"));
    }

    /// <summary>
    /// Logs out a user by invalidating their refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to invalidate.</param>
    /// <returns>A result indicating success or failure of the logout operation.</returns>
    [HttpPost("logout")]
    public async Task<ActionResult<Result<string>>> Logout(string refreshToken)
    {
        var logoutSuccessful = await accountRepository.Logout(refreshToken);
        if (!logoutSuccessful) return BadRequest(Result<string>.FailureResult("Invalid refresh token"));

        return Ok(Result<string>.SuccessResult(null, "Logout successful"));
    }

    /// <summary>
    /// Logs out a user from all devices by invalidating all their refresh tokens.
    /// </summary>
    /// <returns>A result indicating success or failure of the logout operation.</returns>
    [HttpPost("logout-all")]
    public async Task<ActionResult<Result<string>>> LogoutAllDevices()
    {
        var userId = await userManager.Users
            .AsNoTracking()
            .Where(x => x.Id == User.FindFirstValue(ClaimTypes.NameIdentifier))
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
        var logoutSuccessful = await accountRepository.LogoutAllDevices(userId);
        if (!logoutSuccessful) return BadRequest(Result<string>.FailureResult("All devices are already logged out"));

        return Ok(Result<string>.SuccessResult(null, "Logout successful"));
    }

    private async Task<bool> CreateUserObject(AppUser user)
    {
        var token = tokenService.CreateTokenAsync(user);
        var refreshToken = await tokenService.CreateRefreshTokenAsync(user);
        AssignTokensToResponseHeaderAndCookie(token, refreshToken);
        return true;
    }

    private void AssignTokensToResponseHeaderAndCookie(string token, RefreshTokenCookieResponse refreshToken)
    {
        Response.Headers.Append("Authorization", $"Bearer {token}");
        Response.Cookies.Append("RefreshToken", refreshToken.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            Expires = refreshToken.Expires
        });
    }
}