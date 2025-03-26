namespace Domain.Entities;

public class RefreshToken
{
    public string RefreshTokenId { get; set; } = Guid.NewGuid().ToString();
    public string Token { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public DateTime Expires { get; set; }
}