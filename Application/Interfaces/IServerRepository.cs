using Application.DTOs.Invites;
using Application.DTOs.Servers;

namespace Application.Interfaces;

public interface IServerRepository
{
    Task<List<ServerDto>> GetServersByUserId(string userId);
    Task<ServerDto> CreateServer(CreateServerDto createServerDto, string userId);
    Task<ServerBasicDto> GetServerBasicAsync(string serverId);
    Task<ServerDetailsDto> GetServerDetailsAsync(string serverId);
    Task<InviteDto> CreateInvite(CreateInviteDto createInviteDto);
    Task<bool> DeleteServer(string serverId);
}