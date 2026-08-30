namespace FCG.Domain.Entities;

public class Game : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<UserGame>? UserGames { get; set; }
}
