namespace FCG.Application.DTOs.Users;

public class UpdateUserDto
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public long UserRoleId { get; set; }
}
