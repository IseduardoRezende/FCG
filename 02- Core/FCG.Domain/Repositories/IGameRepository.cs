using FCG.Domain.Entities;
using FCG.Domain.Filters;

namespace FCG.Domain.Repositories;

public interface IGameRepository
{
    Task<Game?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Game> Items, int TotalCount)> GetPagedAsync(GameFilter filter, CancellationToken cancellationToken = default);

    Task AddAsync(Game game, CancellationToken cancellationToken = default);

    Task UpdateAsync(Game game, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
