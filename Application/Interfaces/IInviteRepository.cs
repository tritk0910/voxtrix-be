using Application.Core;
using Application.DTOs.Invites;

namespace Application.Interfaces;

public interface IInviteRepository
{
    Task<string> JoinServerViaInviteLinkAsync(string inviteCode, string userId);
    Task<InviteDto> CreateInvite(CreateInviteDto createInviteDto, string userId);
    Task<Result<InviteDto>> UpdateInviteAsync(UpdateInviteDto updateInviteDto);
    Task<Result<bool>> PauseInviteAsync(string serverId);
    Task<Result<bool>> DeleteInviteAsync(string inviteId);
}