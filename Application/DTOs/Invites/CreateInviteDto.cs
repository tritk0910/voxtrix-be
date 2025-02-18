namespace Application.DTOs.Invites;

public class CreateInviteDto
{
    public string ServerId { get; set; }
    public string AuthorId { get; set; }
    public int MaxUses { get; set; }
    public DateTime ExpiredAt { get; set; }
}