using Application.Core;
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

        var everyoneRole = new Role
        {
            RoleName = "everyone",
            Permissions = (long)(RolePermission.ViewChannel | RolePermission.ReadMessageHistory),
            Color = "#000000",
            Position = 0,
            IsDefault = true
        };

        server.Roles = [everyoneRole];


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
            .ProjectTo<ServerBasicDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return server;
    }

    public async Task<ServerDetailsDto> GetServerDetailsAsync(string serverId)
    {
        var server = await context.Servers
            .Where(s => s.ServerId == serverId)
            .AsNoTracking()
            .Include(s => s.Invites)
            .Include(s => s.Roles)
            .ProjectTo<ServerDetailsDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return server;
    }

    public async Task<IQueryable<ServerMemberDto>> GetMembersByServerId(string serverId)
    {
        var members = context.ServerMembers
            .Where(sm => sm.ServerId == serverId)
            .AsNoTracking()
            .ProjectTo<ServerMemberDto>(mapper.ConfigurationProvider)
            .AsQueryable();

        return await Task.FromResult(members);
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
        if (currentOwner.MemberId == newOwnerId) return Result<ServerTransferDto>.FailureResult("You are already the owner of this server");

        var newOwner = server.ServerMembers.FirstOrDefault(sm => sm.MemberId == newOwnerId);
        if (newOwner == null) return Result<ServerTransferDto>.FailureResult("New owner is not a member of this server");

        currentOwner.IsOwner = false;
        newOwner.IsOwner = true;

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return Result<ServerTransferDto>.FailureResult("Failed to transfer ownership");

        return Result<ServerTransferDto>.SuccessResult(mapper.Map<ServerTransferDto>(server), "Ownership transferred successfully");
    }
}
