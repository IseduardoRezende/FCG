namespace FCG.Application.Abstractions;

public interface ICurrentUser
{
    long? UserId { get; }

    string? Role { get; }

    bool IsAdministrator { get; }
}
