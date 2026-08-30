using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly FcgDbContext _dbContext;

    public GameRepository(FcgDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Game?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Game>().FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Game>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Game>().OrderBy(g => g.Name).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Game game, CancellationToken cancellationToken = default)
    {
        _dbContext.ChangeTracker.Clear();
        await _dbContext.Set<Game>().AddAsync(game, cancellationToken);
    }

    public Task UpdateAsync(Game game, CancellationToken cancellationToken = default)
    {
        _dbContext.ChangeTracker.Clear();
        _dbContext.Set<Game>().Update(game);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dbContext.ChangeTracker.Clear();
    }
}
