namespace Application.Interfaces;

public interface IInviteRepository
{
    Task<string> GetInviteAsync(string inviteCode, string userId);
    Task<bool> DeleteInviteAsync(string inviteId);
}