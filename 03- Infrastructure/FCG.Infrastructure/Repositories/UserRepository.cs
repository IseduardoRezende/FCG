using FCG.Domain.Entities;
using FCG.Domain.Filters;
using FCG.Domain.Repositories;
using FCG.Infrastructure.DbContexts;
using FCG.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FCG.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FcgDbContext _dbContext;

    public UserRepository(FcgDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<User>()
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<User>()
            .Include(u => u.UserRole)
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<User>()
            .AnyAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
    }

    public async Task<(IReadOnlyList<User> Items, int TotalCount)> GetPagedAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<User>()
            .Include(u => u.UserRole)
            .Where(ApplyFilter(filter));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .ApplyOrdering(filter)
            .ApplyPagination(filter)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.ChangeTracker.Clear();
        await _dbContext.Set<User>().AddAsync(user, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.ChangeTracker.Clear();
        _dbContext.Set<User>().Update(user);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
        _dbContext.ChangeTracker.Clear();
    }

    private static Expression<Func<User, bool>> ApplyFilter(UserFilter filter)
    {
        var value = filter.Value?.ToLower();

        return u =>
            (filter.UserRoleId == null || u.UserRoleId == filter.UserRoleId) &&
            (string.IsNullOrWhiteSpace(value) ||
             u.Name.ToLower().Contains(value) ||
             u.Email.ToLower().Contains(value));
    }
}
