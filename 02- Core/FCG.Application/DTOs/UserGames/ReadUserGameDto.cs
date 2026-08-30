namespace FCG.Application.DTOs.UserGames;

public class ReadUserGameDto
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string? UserName { get; set; }
    
    public string? UserEmail { get; set; }

    public long GameId { get; set; }

    public string GameName { get; set; } = string.Empty;

    public decimal GamePrice { get; set; }

    public DateTime PurchasedAt { get; set; }
}
