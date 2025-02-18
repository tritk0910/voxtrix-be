using Application.Core;
using Application.DTOs.Invites;
using Application.DTOs.Servers;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories;

public class ServerRepository(DataContext context, IMapper mapper) : IServerRepository
{
    public async Task<List<ServerDto>> GetServersByUserId(string userId)
    {
        var servers = await context.Servers
            .Where(s => s.ServerMembers.Any(sm => sm.MemberId == userId))
            .AsNoTracking()
            .ProjectTo<ServerDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return servers;
    }

    public async Task<ServerDto> CreateServer(CreateServerDto createServerDto, string userId)
    {
        var server = mapper.Map<Server>(createServerDto);
        server.OwnerId = userId;
        server.ServerMembers =
        [
            new ServerMember
            {
                ServerMemberId = Guid.NewGuid().ToString(),
                MemberId = userId,
                IsOwner = true
            }
        ];

        context.Servers.Add(server);
        await context.SaveChangesAsync();

        return mapper.Map<ServerDto>(server);
    }

    public async Task<ServerBasicDto> GetServerBasicAsync(string serverId)
    {
        var server = await context.Servers
            .Where(s => s.ServerId == serverId)
            .AsNoTracking()
            .Include(s => s.ServerMembers)
                .ThenInclude(sm => sm.Member)
            .ProjectTo<ServerBasicDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return server;
    }

    public async Task<ServerDetailsDto> GetServerDetailsAsync(string serverId)
    {
        var server = await context.Servers
            .Where(s => s.ServerId == serverId)
            .AsNoTracking()
            .Include(s => s.ServerMembers)
                .ThenInclude(sm => sm.Member)
            .Include(s => s.Invites)
            .ProjectTo<ServerDetailsDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return server;
    }

    public async Task<Result<bool>> DeleteServer(string userId, string serverId)
    {
        var server = await context.Servers.FindAsync(serverId);
        if (server == null) return Result<bool>.FailureResult("Server not found");

        if (server.OwnerId != userId) return Result<bool>.FailureResult("You are not the owner of this server");

        context.Servers.Remove(server);
        var result = await context.SaveChangesAsync() > 0;

        if (!result) return Result<bool>.FailureResult("Failed to delete server");

        return Result<bool>.SuccessResult(true, "Server deleted successfully");
    }

    public async Task<Result<ServerTransferDto>> TransferOwnership(string userId, string serverId, string newOwnerId)
    {
        var server = await context.Servers
            .Include(s => s.ServerMembers)
            .FirstOrDefaultAsync(s => s.ServerId == serverId);
        if (server == null) return Result<ServerTransferDto>.FailureResult("Server not found");

        var currentOwner = server.ServerMembers.FirstOrDefault(sm => sm.IsOwner);
        if (currentOwner == null || currentOwner.MemberId != userId) return Result<ServerTransferDto>.FailureResult("You are not the owner of this server");

        var newOwner = server.ServerMembers.FirstOrDefault(sm => sm.MemberId == newOwnerId);
        if (newOwner == null) return Result<ServerTransferDto>.FailureResult("New owner is not a member of this server");

        currentOwner.IsOwner = false;
        newOwner.IsOwner = true;

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return Result<ServerTransferDto>.FailureResult("Failed to transfer ownership");

        return Result<ServerTransferDto>.SuccessResult(mapper.Map<ServerTransferDto>(server), "Ownership transferred successfully");
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
