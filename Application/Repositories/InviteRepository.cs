using Application.Core;
using Application.DTOs.Invites;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories;

public class InviteRepository(DataContext context, IMapper mapper) : IInviteRepository
{
    public async Task<string> JoinServerViaInviteLinkAsync(string inviteCode, string userId)
    {
        var result = await context.Invites
            .Where(i => i.InviteCode == inviteCode)
            .FirstOrDefaultAsync();

        if (result == null) return "Invite not found";
        if (result.IsPaused) return "Invite is paused";

        // Check if the invite link is expired
        if (result.ExpiredAt < DateTime.UtcNow) return "Invite link is expired";

        // Check if the user is banned from the server
        var isBanned = await context.ServerBans
            .AnyAsync(b => b.ServerId == result.ServerId && b.UserId == userId && b.UnbannedAt > DateTime.UtcNow);

        if (isBanned) return "User is banned from the server";

        // Check if the user is already in the server
        var isMember = await context.ServerMembers
            .AnyAsync(sm => sm.ServerId == result.ServerId && sm.MemberId == userId);
        if (isMember) return "User is already a member of the server";

        // Check if the invite has remaining uses or is unlimited
        if (result.MaxUses != -1 && result.Uses >= result.MaxUses) return "Invite has no remaining uses";

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

    public async Task<InviteDto> CreateInvite(CreateInviteDto createInviteDto)
    {
        var invite = mapper.Map<Invite>(createInviteDto);
        invite.InviteCode = GenerateRandomString();
        invite.MaxUses = -1;
        invite.ExpiredAt = DateTime.UtcNow.AddHours(24);

        invite.Author = await context.Users
            .Where(u => u.Id == invite.AuthorId)
            .SingleOrDefaultAsync();

        context.Invites.Add(invite);
        await context.SaveChangesAsync();

        return mapper.Map<InviteDto>(invite);
    }

    public async Task<Result<InviteDto>> UpdateInviteAsync(UpdateInviteDto updateInviteDto)
    {
        var invite = await context.Invites.FirstOrDefaultAsync(i => i.InviteId == updateInviteDto.InviteId);

        if (invite == null) return Result<InviteDto>.FailureResult("Invite not found");
        mapper.Map(updateInviteDto, invite);

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return Result<InviteDto>.FailureResult("Failed to update invite");

        var inviteDto = mapper.Map<InviteDto>(invite);
        return Result<InviteDto>.SuccessResult(inviteDto, "Invite updated successfully");
    }

    public async Task<Result<bool>> PauseInviteAsync(string serverId)
    {
        var invites = await context.Invites.Where(i => i.ServerId == serverId).ToListAsync();

        if (invites.Count == 0) return Result<bool>.FailureResult("No invites found");

        foreach (var invite in invites)
        {
            invite.IsPaused = true;
        }

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return Result<bool>.FailureResult("Failed to pause invites");

        return Result<bool>.SuccessResult(true, "Invites paused");
    }

    public async Task<Result<bool>> DeleteInviteAsync(string inviteId)
    {
        var invite = await context.Invites
            .FirstOrDefaultAsync(i => i.InviteId == inviteId);

        if (invite == null) return Result<bool>.FailureResult("Invite not found");

        context.Invites.Remove(invite);
        var result = await context.SaveChangesAsync() > 0;

        if (!result) return Result<bool>.FailureResult("Failed to delete invite");

        return Result<bool>.SuccessResult(true, "Invite deleted successfully");
    }

    private static string GenerateRandomString()
    {
        var _random = new Random();
        int length = _random.Next(8, 11); // Length will be between 8 and 10
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        char[] stringChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[_random.Next(chars.Length)];
        }

        return new string(stringChars);
    }
}