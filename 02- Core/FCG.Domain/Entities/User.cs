namespace FCG.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public long UserRoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public UserRole? UserRole { get; set; }

    public ICollection<UserGame>? UserGames { get; set; }
}
