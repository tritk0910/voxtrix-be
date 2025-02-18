namespace Application.DTOs.Invites;

public class UpdateInviteDto
{
    public string InviteId { get; set; }
    public int MaxUses { get; set; }
    public DateTime ExpiredAt { get; set; }
}