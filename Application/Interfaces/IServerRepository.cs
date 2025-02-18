using Application.Core;
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
    Task<Result<InviteDto>> UpdateInviteAsync(UpdateInviteDto updateInviteDto);
    Task<Result<bool>> PauseInviteAsync(string serverId);
    Task<Result<bool>> DeleteInviteAsync(string inviteId);
    Task<Result<bool>> DeleteServer(string userId, string serverId);
    Task<Result<ServerTransferDto>> TransferOwnership(string userId, string serverId, string newOwnerId);
}