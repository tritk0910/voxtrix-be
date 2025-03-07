using Application.Core;
using Application.DTOs.Servers;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories;

public class ServerRepository(DataContext context, IMapper mapper, ICloudinaryService cloudinaryService) : IServerRepository
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

    public async Task<Result<ServerDto>> CreateServer(CreateServerDto createServerDto, string userId)
    {
        var server = mapper.Map<Server>(createServerDto);
        server.OwnerId = userId;

        if (createServerDto.Avatar != null)
        {
            if (!CloudinaryService.IsAvatarSquareResolutionValid(createServerDto.Avatar, 250, 250))
                return Result<ServerDto>.FailureResult("Image resolution must not exceed 250x250");

            var uploadResult = await cloudinaryService.UploadImageAsync(createServerDto.Avatar);
            if (uploadResult.Error != null) return Result<ServerDto>.FailureResult(uploadResult.Error.Message);

            server.Avatar = uploadResult.SecureUrl.AbsoluteUri;
        }

        var everyoneRole = new Role
        {
            RoleName = "@everyone",
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
        var result = await context.SaveChangesAsync() > 0;
        if (!result) return Result<ServerDto>.FailureResult("Failed to create server");
        return Result<ServerDto>.SuccessResult(mapper.Map<ServerDto>(server), "Server created successfully");
    }

    public async Task<Result<ServerDto>> UpdateServer(EditServerDto editServerDto, string userId)
    {
        var server = await context.Servers.FindAsync(editServerDto.Id);
        if (server == null) return Result<ServerDto>.FailureResult("Server not found");

        if (server.OwnerId != userId) return Result<ServerDto>.FailureResult("You are not the owner of this server");

        mapper.Map(editServerDto, server);

        if (editServerDto.Avatar != null && !editServerDto.ResetAvatar)
        {
            if (!CloudinaryService.IsAvatarSquareResolutionValid(editServerDto.Avatar, 250, 250))
                return Result<ServerDto>.FailureResult("Image resolution must not exceed 250x250");

            var uploadResult = await cloudinaryService.UploadImageAsync(editServerDto.Avatar);
            if (uploadResult.Error != null) return Result<ServerDto>.FailureResult(uploadResult.Error.Message);

            server.Avatar = uploadResult.SecureUrl.AbsoluteUri;
        }

        if (editServerDto.ResetAvatar)
        {
            server.Avatar = null;
        }

        var result = await context.SaveChangesAsync() > 0;
        if (!result) return Result<ServerDto>.FailureResult("There's nothing to update");
        return Result<ServerDto>.SuccessResult(mapper.Map<ServerDto>(server), "Server updated successfully");
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

    public async Task<IQueryable<ServerMemberDto>> GetMembersByServerId(string serverId, DefaultParams defaultParams)
    {
        var members = context.ServerMembers
            .Where(sm => sm.ServerId == serverId)
            .AsNoTracking()
            .Include(m => m.Member)
            .AsQueryable();

        if (!string.IsNullOrEmpty(defaultParams.Search))
        {
            members = members.Where(m => m.Member.UserName.ToLower().Contains(defaultParams.Search.ToLower()));
        }

        var result = members
            .ProjectTo<ServerMemberDto>(mapper.ConfigurationProvider);

        return await Task.FromResult(result);
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

    public async Task<Result<ServerDto>> CreateServerByScript(CreateServerByScriptDto createServerByScriptDto, string userId)
    {
        if (createServerByScriptDto.Script == default)
            return Result<ServerDto>.FailureResult("No script provided");

        var server = new Server { };

        if (createServerByScriptDto.Avatar != null)
        {
            if (!CloudinaryService.IsAvatarSquareResolutionValid(createServerByScriptDto.Avatar, 250, 250))
                return Result<ServerDto>.FailureResult("Image resolution must not exceed 250x250");

            var uploadResult = await cloudinaryService.UploadImageAsync(createServerByScriptDto.Avatar);
            if (uploadResult.Error != null) return Result<ServerDto>.FailureResult(uploadResult.Error.Message);

            server.Avatar = uploadResult.SecureUrl.AbsoluteUri;
        }

        await CreateServerByScriptInternal(server, createServerByScriptDto, userId);

        return Result<ServerDto>.SuccessResult(mapper.Map<ServerDto>(server), "Server created successfully");
    }

    private async Task<Server> CreateServerByScriptInternal(Server server, CreateServerByScriptDto createServerByScriptDto, string userId)
    {
        var everyoneRole = new Role
        {
            RoleName = "@everyone",
            Permissions = (long)(RolePermission.ViewChannel | RolePermission.ReadMessageHistory),
            Color = "#000000",
            Position = 0,
            IsDefault = true
        };

        server.ServerName = createServerByScriptDto.Name;
        server.OwnerId = userId;
        server.Roles = [everyoneRole];
        server.ServerMembers =
        [
            new ServerMember
                {
                    MemberId = userId,
                    IsOwner = true,
                    ServerMemberRoles = [new ServerMemberRole { RoleId = everyoneRole.RoleId }]
                }
        ];

        context.Servers.Add(server);
        await context.SaveChangesAsync();

        var categories = new List<Channel> {
            new()
                {
                    ChannelName = "Text channels",
                    ServerId = server.ServerId,
                    ChannelType = ChannelType.Category,
                },
                new()
                {
                    ChannelName = "Voice channels",
                    ServerId = server.ServerId,
                    ChannelType = ChannelType.Category,
                },
            };

        await context.Channels.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        var channels = new List<Channel> { };
        switch (createServerByScriptDto.Script)
        {
            default:
            case ServerScript.Basic:
                channels = [
                    new()
                    {
                        ChannelName = "general",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Text,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Text channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "general",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Voice,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Voice channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                ];
                await context.Channels.AddRangeAsync(channels);
                await context.SaveChangesAsync();
                break;

            case ServerScript.Gaming:
                channels =
                [
                    new()
                    {
                        ChannelName = "general",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Text,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Text channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "clips-and-highlights",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Text,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Text channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "Lobby",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Voice,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Voice channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "Gaming",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Voice,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Voice channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                ];
                await context.Channels.AddRangeAsync(channels);
                await context.SaveChangesAsync();
                break;

            case ServerScript.StudyGroup:
                var additionalCategory = new Channel
                {
                    ChannelName = "Information",
                    ServerId = server.ServerId,
                    ChannelType = ChannelType.Category,
                };

                await context.Channels.AddAsync(additionalCategory);
                await context.SaveChangesAsync();

                channels =
                [
                    new()
                    {
                        ChannelName = "welcome-and-rules",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Text,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Information" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "notes-resources",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Text,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Information" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "general",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Text,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Text channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "homework-help",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Text,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Text channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "session-planning",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Text,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Text channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "off-topic",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Text,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Text channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "Lounge",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Voice,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Voice channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "Study Room 1",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Voice,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Voice channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                    new()
                    {
                        ChannelName = "Study Room 2",
                        ServerId = server.ServerId,
                        ChannelType = ChannelType.Voice,
                        ParentChannelId = context.Channels.FirstOrDefault(x => x.ChannelName == "Voice channels" && x.ServerId == server.ServerId)?.ChannelId
                    },
                ];
                await context.Channels.AddRangeAsync(channels);
                await context.SaveChangesAsync();
                break;
        }

        return server;
    }
}
