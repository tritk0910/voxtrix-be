namespace Application.DTOs.Servers;

public class BanDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string ServerId { get; set; }
    public string Reason { get; set; }
    public DateTime BannedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
}