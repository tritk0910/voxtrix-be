using Application.Core;
using Application.DTOs.Servers;

namespace Application.Interfaces;

public interface IServerRepository
{
    Task<List<ServerDto>> GetServersByUserId(string userId);
    Task<Result<ServerDto>> CreateServer(CreateServerDto createServerDto, string userId);
    Task<Result<ServerDto>> UpdateServer(EditServerDto editServerDto, string userId);
    Task<ServerBasicDto> GetServerBasicAsync(string serverId);
    Task<ServerDetailsDto> GetServerDetailsAsync(string serverId);
    Task<IQueryable<ServerMemberDto>> GetMembersByServerId(string serverId, DefaultParams defaultParams);
    Task<Result<bool>> DeleteServer(string userId, string serverId);
    Task<Result<ServerTransferDto>> TransferOwnership(string userId, string serverId, string newOwnerId);
    Task<Result<ServerDto>> CreateServerByScript(CreateServerByScriptDto createServerByScriptDto, string userId);
}