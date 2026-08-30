namespace FCG.Application.DTOs.Users;

public class ReadUserDto
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public long UserRoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
