using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories;

public class InviteRepository(DataContext context) : IInviteRepository
{
    public async Task<bool> DeleteInviteAsync(string inviteId)
    {
        var result = await context.Invites
            .Where(i => i.InviteId == inviteId)
            .FirstOrDefaultAsync();

        if (result != null)
        {
            context.Invites.Remove(result);
            await context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<string> GetInviteAsync(string inviteCode, string userId)
    {
        var result = await context.Invites
            .Where(i => i.InviteCode == inviteCode)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return "Invite not found";
        }

        // Check if the user is banned from the server
        var isBanned = await context.ServerBans
            .AnyAsync(b => b.ServerId == result.ServerId && b.UserId == userId && b.UnbannedAt > DateTime.UtcNow);

        if (isBanned)
        {
            return "User is banned from the server";
        }

        // Check if the user is already in the server
        var isMember = await context.ServerMembers
            .AnyAsync(sm => sm.ServerId == result.ServerId && sm.MemberId == userId);

        if (isMember)
        {
            return "User is already a member of the server";
        }

        // Check if the invite has remaining uses or is unlimited
        if (result.MaxUses != null && result.Uses >= result.MaxUses)
        {
            return "Invite has no remaining uses";
        }

        // Add the user to the server
        var serverMember = new ServerMember
        {
            ServerMemberId = Guid.NewGuid().ToString(),
            ServerId = result.ServerId,
            MemberId = userId,
            JoinedAt = DateTime.UtcNow
        };

        context.ServerMembers.Add(serverMember);

        // Increment the invite uses
        result.Uses++;
        await context.SaveChangesAsync();

        return "User added to the server successfully";
    }
}