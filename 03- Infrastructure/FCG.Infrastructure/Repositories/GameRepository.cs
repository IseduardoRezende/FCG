using FCG.Domain.Entities;
using FCG.Domain.Filters;
using FCG.Domain.Repositories;
using FCG.Infrastructure.DbContexts;
using FCG.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public async Task<(IReadOnlyList<Game> Items, int TotalCount)> GetPagedAsync(GameFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<Game>()
            .Where(ApplyFilter(filter));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .ApplyOrdering(filter)
            .ApplyPagination(filter)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
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

    private static Expression<Func<Game, bool>> ApplyFilter(GameFilter filter)
    {
        var value = filter.Value?.ToLower();

        return g =>
            (string.IsNullOrWhiteSpace(value) ||
             g.Name.ToLower().Contains(value) ||
             g.Description.ToLower().Contains(value)) &&
            (filter.MinPrice == null || g.Price >= filter.MinPrice) &&
            (filter.MaxPrice == null || g.Price <= filter.MaxPrice);
    }
}
