using FCG.Domain.Entities;

namespace FCG.Domain.Repositories;

public interface IGameRepository
{
    Task<Game?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Game>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Game game, CancellationToken cancellationToken = default);

    Task UpdateAsync(Game game, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
