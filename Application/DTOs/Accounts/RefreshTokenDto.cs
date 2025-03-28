namespace Application.DTOs.Accounts;

public class RefreshTokenDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public class RefreshTokenCookieResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}