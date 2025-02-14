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
        invite.MaxUses = null;

        context.Invites.Add(invite);
        await context.SaveChangesAsync();

        return mapper.Map<InviteDto>(invite);
    }
}
