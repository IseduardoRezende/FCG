using FCG.Domain.Entities;

namespace FCG.Domain.Repositories;

public interface IUserGameRepository
{
    Task<UserGame?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long userId, long gameId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UserGame>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);

    Task AddAsync(UserGame userGame, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
