using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services;

public class AccountRepository(DataContext context) : IAccountRepository
{
    public async Task<bool> Logout(string refreshToken)
    {
        var existingToken = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        if (existingToken == null) return false;

        context.RefreshTokens.Remove(existingToken);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> LogoutAllDevices(string userId)
    {
        var existingTokens = await context.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();
        if (existingTokens.Count == 0) return false;

        context.RefreshTokens.RemoveRange(existingTokens);
        await context.SaveChangesAsync();

        return true;
    }
}