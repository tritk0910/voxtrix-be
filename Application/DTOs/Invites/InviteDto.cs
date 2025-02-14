namespace Application.DTOs.Invites;

public class InviteDto
{
    public string Id { get; set; }
    public string Code { get; set; }
    public string ServerId { get; set; }
    public string AuthorId { get; set; }
    public int MaxUses { get; set; }
    public int Uses { get; set; }
    public DateTime ExpiredAt { get; set; }
}