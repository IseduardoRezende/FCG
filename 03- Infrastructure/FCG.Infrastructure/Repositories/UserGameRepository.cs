using FCG.Domain.Entities;
using FCG.Domain.Filters;
using FCG.Domain.Repositories;
using FCG.Infrastructure.DbContexts;
using FCG.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FCG.Infrastructure.Repositories;

public class UserGameRepository : IUserGameRepository
{
    private readonly FcgDbContext _dbContext;

    public UserGameRepository(FcgDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserGame?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UserGame>()
            .Include(ug => ug.Game)
            .Include(ug => ug.User)
            .FirstOrDefaultAsync(ug => ug.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(long userId, long gameId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UserGame>()
            .AnyAsync(ug => ug.UserId == userId && ug.GameId == gameId, cancellationToken);
    }

    public async Task<(IReadOnlyList<UserGame> Items, int TotalCount)> GetPagedAsync(UserGameFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<UserGame>()
            .Include(ug => ug.Game)
            .Where(ApplyFilter(filter));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .ApplyOrdering(filter)
            .ApplyPagination(filter)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(UserGame userGame, CancellationToken cancellationToken = default)
    {
        _dbContext.ChangeTracker.Clear();
        await _dbContext.Set<UserGame>().AddAsync(userGame, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dbContext.ChangeTracker.Clear();
    }

    private static Expression<Func<UserGame, bool>> ApplyFilter(UserGameFilter filter)
    {
        var value = filter.Value?.ToLower();

        return ug =>
            (filter.UserId == null || ug.UserId == filter.UserId) &&
            (filter.GameId == null || ug.GameId == filter.GameId) &&
            (filter.PurchasedFrom == null || ug.PurchasedAt >= filter.PurchasedFrom) &&
            (filter.PurchasedTo == null || ug.PurchasedAt <= filter.PurchasedTo) &&
            (string.IsNullOrWhiteSpace(value) || ug.Game!.Name.ToLower().Contains(value));
    }
}
