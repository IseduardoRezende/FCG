using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IReadOnlyList<UserGame>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<UserGame>()
            .Include(ug => ug.Game)
            .Where(ug => ug.UserId == userId)
            .OrderByDescending(ug => ug.PurchasedAt)
            .ToListAsync(cancellationToken);
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
}
