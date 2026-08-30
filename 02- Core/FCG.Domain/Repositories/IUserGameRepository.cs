using FCG.Domain.Entities;
using FCG.Domain.Filters;

namespace FCG.Domain.Repositories;

public interface IUserGameRepository
{
    Task<UserGame?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long userId, long gameId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<UserGame> Items, int TotalCount)> GetPagedAsync(UserGameFilter filter, CancellationToken cancellationToken = default);

    Task AddAsync(UserGame userGame, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
