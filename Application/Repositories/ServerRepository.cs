using Application.DTOs.Invites;
using Application.DTOs.Servers;
using Application.DTOs.Users;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal;
using Persistence;

namespace Application.Repositories;

public class ServerRepository(DataContext context, IMapper mapper) : IServerRepository
{
    public async Task<List<ServerDto>> GetServersByUserId(string userId)
    {
        var servers = await context.Servers
            .Where(s => s.ServerMembers.Any(sm => sm.MemberId == userId))
            .ProjectTo<ServerDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
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
            .Include(s => s.ServerMembers)
            .ThenInclude(sm => sm.Member)
            .ProjectTo<ServerBasicDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return server;
    }

    public async Task<ServerDetailsDto> GetServerDetailsAsync(string serverId)
    {
        var server = await context.Servers
            .Where(s => s.ServerId == serverId)
            .Include(s => s.ServerMembers)
                .ThenInclude(sm => sm.Member)
            .Include(s => s.Invites)
            .ProjectTo<ServerDetailsDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return server;
    }

    public async Task<bool> DeleteServer(string serverId)
    {
        var server = await context.Servers.FindAsync(serverId);

        if (server == null) return false;

        context.Servers.Remove(server);
        await context.SaveChangesAsync();

        return true;
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

    public async Task<bool> UpdateInviteAsync(UpdateInviteDto updateInviteDto)
    {
        var invite = await context.Invites.FirstOrDefaultAsync(i => i.InviteId == updateInviteDto.InviteId);

        if (invite == null) return false;
        mapper.Map(updateInviteDto, invite);

        var result = await context.SaveChangesAsync() > 0;

        return result;
    }

    public async Task<string> PauseInviteAsync(string serverId)
    {
        var invites = await context.Invites.Where(i => i.ServerId == serverId).ToListAsync();

        if (invites.Count == 0) return "No invites found";

        foreach (var invite in invites)
        {
            invite.IsPaused = true;
        }

        var result = await context.SaveChangesAsync() > 0;

        if (result) return "Invites paused";
        return "Failed to pause invites";
    }

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
