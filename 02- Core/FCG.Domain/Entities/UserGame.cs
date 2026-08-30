namespace FCG.Domain.Entities;

public class UserGame : BaseEntity
{
    public long UserId { get; set; }

    public long GameId { get; set; }

    public DateTime PurchasedAt { get; set; }

    public User? User { get; set; }

    public Game? Game { get; set; }
}
