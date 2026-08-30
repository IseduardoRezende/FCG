namespace FCG.Application.DTOs.Users;

public class TokenDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}
