namespace Application.Interfaces;

public interface IInviteRepository
{
    Task<string> JoinServerViaInviteLinkAsync(string inviteCode, string userId);
}