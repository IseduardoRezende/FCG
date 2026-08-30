namespace FCG.Application.DTOs.Games;

public class CreateGameDto
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
